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
        private readonly JWTContext _context;
        private readonly UserViewModel _personService;

        public ValuesController(JWTContext context, UserViewModel personService)
        {
            _context = context;
            _personService = personService;
        }

        
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(User request)
        {
            try
            {
                return Ok(await _personService.Register(request));
            }
            catch (Exception ex)
            {
                return BadRequest($"Bad'{ex}'");
            }
        }
            [HttpPost("Login")]
        public async Task<ActionResult<User>> Register(UserLog request)
        {
            try
            {
                var red = await _personService.Login(request);
                if (red != null)
                {
                    return Ok("User Logged In!"+red);

                }
                else
                {
                    return BadRequest("Invalid Credentials!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Bad'{ex}'");
            }
           
        }
        
    }
}
