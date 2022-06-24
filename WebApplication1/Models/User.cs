using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
//using WebApplication1.Data;

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

        

        public UserViewModel(JWTContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<JsonResult> Register(User request)
        {
            var log = _context.Users.Where(x => x.Username.Equals(request.Username)).FirstOrDefault();

          
            if (log == null)
            {
                _context.Users.Add(request);
                await _context.SaveChangesAsync();
                return new JsonResult("Signup Sucess");
            }
            else
                
             return new JsonResult("Username Already in Use!");

        }
        
        public async Task<JsonResult> Login(UserLog request)
        {

            var log = _context.Users.Where(x => x.Username.Equals(request.UserName) && x.Password.Equals(request.Password)).FirstOrDefault();
            if (log == null)
            {

                return new JsonResult("Login Failed!");
            }
            else
            {
                
                string token = CreateToken(request);
                
                return new JsonResult ("User Login successfully! \n" +token);
            }
            

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
