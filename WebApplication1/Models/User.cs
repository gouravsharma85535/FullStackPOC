using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace WebApplication1.Models
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
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }
      
        public async Task<JsonResult> Register(User request)
        {
            var log = _context.Users.Where(x => x.Username.Equals(request.Username)).FirstOrDefault();
            if (log == null)
            {
                _context.Users.Add(request);
                await _context.SaveChangesAsync();
                return new JsonResult(new { message = "SignUp Successful!", status = 200 });
            }
            else
                return new JsonResult(new{ message = "Username Already in Use!", status=400 });
        }
        
        public async Task<JsonResult> Login(UserLog request)
        {
            var log = _context.Users.Where(x => x.Username.Equals(request.UserName) && x.Password.Equals(request.Password)).FirstOrDefault();
           
            if (log == null)
            {
                 return new JsonResult(new { message = "No User Found! Enter Correctly!", status = 400 });
            }
            else
            {
              string token = CreateToken(request);
                _memoryCache.Set(key: "mu",value: token, TimeSpan.FromMinutes(1));
                _httpContextAccessor.HttpContext.Session.SetString(key: "myKey", value: token);
                _httpContextAccessor.HttpContext.Response.Headers.Add(key: "x-custom-headerw", value: token);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token",token, new CookieOptions { HttpOnly = true });
                return new(new { message = "Login Successful!", status = 200 });
            }
        }
        public async Task<JsonResult> Get()
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
            var jsonToken = handler.ReadToken(authHeader);
            var TS = handler.ReadToken(authHeader) as JwtSecurityToken;
            var id = TS.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var r = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
            var c = _memoryCache.Get(key: "mu");
            if (r == c.ToString())
                return new( _context.Users.FindAsync(id));
            else
                return new("Didnt Match!");
        }
       

        private string CreateToken(UserLog usr)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, (usr.UserName)),
               
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretKey!753159"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }
    }

}
