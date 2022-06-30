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
        
        public async Task<object> Login(UserLog request)
        {
            var log = _context.Users.Where(x => x.Username.Equals(request.UserName) && x.Password.Equals(request.Password)).FirstOrDefault();
           
            if (log == null)
            {
                 return new JsonResult(new {success=0, message = "No User Found! Enter Correctly!", status = 400 });
            }
            else
            {
              string token = CreateToken(request);
                _memoryCache.Set(key: "mu",value: token, TimeSpan.FromMinutes(1));
                _httpContextAccessor.HttpContext.Response.Cookies.Append("bearer_token", token, new CookieOptions { HttpOnly = false }); ;
                return new JsonResult(new {success=1, message = "Login Successful!", status = 200, token=token });
            }
        }
        public async Task<JsonResult> Get()
        {

            var c = _memoryCache.Get(key: "mu");
            var k = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("bearer_token", out var val);
            string header = _httpContextAccessor.HttpContext.Request.Headers["bearer_token"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(header);
            var claimValue = securityToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string z = val.ToString();
            if (header == c.ToString())
            {
                return new(new { message = "Sucessfull", Data = await _context.Users.FindAsync(claimValue) });
            }
            else
                return new(new { message="Failed" , Data=val + "                   " + header + "               " + c});
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
