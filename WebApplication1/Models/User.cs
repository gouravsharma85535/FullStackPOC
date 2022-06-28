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
//using System.Text.Json;
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
        //ISession session;
        private readonly JWTContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticatedResponse loga;
        private readonly IMemoryCache _memoryCache;
       
            


        public UserViewModel(JWTContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)//,ISession session)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            // this.session = session;
        }
        const string sessionKey = "FirstSeen";
        DateTime dateFirstSeen;
        string value { get; set; }
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
                return new(_context.Users.FindAsync(request.Username));
             return new JsonResult("Username Already in Use!");

        }
        
        public async Task<string> Login(UserLog request)
        {

            var log = _context.Users.Where(x => x.Username.Equals(request.UserName) && x.Password.Equals(request.Password)).FirstOrDefault();
           
            if (log == null)
            {
                //return  null;
                return ("Login Failed!");
            }
            else
            {
              string token = CreateToken(request);
                _memoryCache.Set(key: "mu",value: token, TimeSpan.FromMinutes(1));
                _httpContextAccessor.HttpContext.Session.SetString(key: "myKey", value: token);
                _httpContextAccessor.HttpContext.Response.Headers.Add(key: "x-custom-headerw", value: token);
                //_httpContextAccessor.HttpContext.Session.SetString("token", token);
                //return new JsonResult(new { status = 200, Success = 1, message = "Sucess",token= token });
                // _httpContextAccessor.HttpContext.Session["token"] = token;
                // object? v =r.Token=(token);
                //_httpContextAccessor.HttpContext.Session.SetString("mykey", token);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token",token, new CookieOptions { HttpOnly = true });
                // value = token;
                // var value=_httpContextAccessor.HttpContext.Session.GetString(token);  
                return new(token);
                //return new JsonResult ("User Login successfully! \n" +token);
            }
            //return (log);

        }
        public async Task<JsonResult> GEt(UserLog request)
        {
            //ring token = CreateToken(request);

            //return jwt;
            //erLog us = new UserLog();
            var r = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];//.GetTypedHeaders().Get<string>("access_token");//Request.Cookies.ContainsKey("access_token");
           var d=_httpContextAccessor.HttpContext.Session.GetString("myKey");
            var c=_memoryCache.Get(key: "mu");
            if(r==c.ToString())
            {
              return new JsonResult(await _context.Users.FindAsync(request.UserName));
            }
                      //return (d+"\n"+ c+" \n"+r);
            //if (d == r)
            //    return _context.Users.FindAsync(request.UserName);
            else
                return new JsonResult ( "\n" + c + " \n\n" + r);
            //var claim = _httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(d =>
            //         d.Type == ClaimTypes.Name);
            //return claim.Value.ToString();
           // var v=_httpContextAccessor.HttpContext.Session.GetString("mykey");
           // if (r.Equals(token))
           //   return r;
           //else
           //  return "False";
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
