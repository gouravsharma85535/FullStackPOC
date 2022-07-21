using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tokenController : ControllerBase
    {
        private readonly JWTContext _context;
        private readonly UserViewModel _personService;

        public tokenController(JWTContext context, UserViewModel personService)
        {
            _context = context;
            _personService = personService;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _personService.Get());
            }
            catch (Exception ex)
            {
                return BadRequest($"Bad'{ex}'");
            }
        }
    }
}
