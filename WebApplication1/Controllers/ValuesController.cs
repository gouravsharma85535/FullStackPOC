using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public static User us = new User();
        public static UserLog log = new UserLog();
        private readonly JWTContext _context;

        public ValuesController(JWTContext context)
        {
            _context = context;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(User request)
        {
            _context.Users.Add(request);
            await _context.SaveChangesAsync();
            return Ok(us);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Register(UserLog request)
        {
            var result = from s in _context.Users
                         where s.Username.Equals(request.UserName)
                         select s;
            int i=1, j=1;
            string st = " ";
            foreach (var sc in result)
            {
                if (result != null)
                {
                    var result1 = from s1 in _context.Users
                                  where s1.Pass.Equals(request.Password)
                                  select s1;
                    foreach (var f in result1)
                    {
                        if (result1 != null)//result1.ToString().Contains(request.Password))
                        {
                            string token = CreateToken(request);
                            i = j = 1;
                            return Ok("Login Sucessful \n" + token);
                        }
                        else if(result1 == null)
                        {
                            j = 0;
                            st = "No Password";
                            return BadRequest("No Password");

                        }
                    }
                }
                else if (result == null)
                {
                    st = "No Username";
                    i = 0;
                    return BadRequest("No Username");


                }
            }
            return BadRequest(st);
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
