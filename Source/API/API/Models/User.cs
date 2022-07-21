using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Models
{
    public partial class User
    {
        [Key]
        public string Username { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class UserViewModel
    {
        private readonly JWTContext _context; 
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
       

        public UserViewModel(JWTContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _context = context; // constructor for Database Acccess
            _httpContextAccessor = httpContextAccessor;// Constructor for Headers And Cookies
            _memoryCache = memoryCache;// Constructor for Cache 
        }
      
        public async Task<JsonResult> Register(User request)
        {
            var log = _context.Users.Where(x => x.Username.Equals(request.Username)).FirstOrDefault(); // Linq to check for exeisting Username
            if (log == null)
            {
                _context.Users.Add(request);
                await _context.SaveChangesAsync();
                return new JsonResult(new {success=1, message = "SignUp Successful!", status = 200 });
            }
            else
                return new JsonResult(new{success=0, message = "Username Already in Use!", status=400 });
        }
        
        public async Task<object> Login(UserLog request)
        {
            var log = _context.Users.Where(x => x.Username.Equals(request.UserName) && x.Password.Equals(request.Password)).FirstOrDefault();//Linq to check username and password in db
           
            if (log == null)
            {
                 return new JsonResult(new {success=0, message = "No User Found! Enter Correctly!", status = 400 });
            }
            else
            {
              string token = CreateToken(request);// Calling function to create token
                _memoryCache.Set(key: "mu",value: token, TimeSpan.FromMinutes(1));// Storing token in cache memory with expiration after 1 min
                
                return new JsonResult(new {success=1, message = "Login Successful!", status = 200, token=token });
            }
        }
        public async Task<JsonResult> Get()
        {

            var c = _memoryCache.Get(key: "mu");// Getting the stored Cache memory token
          
            string header = _httpContextAccessor.HttpContext.Request.Headers["bearer_token"];// Gdtting the token set by frontend from request-headers

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(header);//Reading the Token as JWT
            var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;// Identifying the claim based on payload from the securityToken
           
            if (header == c.ToString())
            {
                return new(new { message = "Sucessfull", Data = await _context.Users.FindAsync(claimValue) });
            }
            else
                return new(new { message="Failed" , Data=header + "               " + c});
        }
       

        private string CreateToken(UserLog usr)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, (usr.UserName)),//setting the claim as username as its unique 
               
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKey!753159")); //Encoding the Secret key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);// Encryting the signature
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);// Adding the secret key by encrypting it to the header, signature  to  create a token
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);// Making the Token As JWT
            
            return jwt;
        }
    }

}
