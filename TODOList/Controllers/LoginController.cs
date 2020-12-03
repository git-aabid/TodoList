using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TODO.Contacts.Services;
using TODO.Models;

namespace TODOList.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly byte[] _key;
        public LoginController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _key = Encoding.ASCII.GetBytes(config.GetSection("AppSettings")["Secret"].ToCharArray());
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Login(UserModel model)
        {
            if (ModelState.IsValid)
            {
                // validate login
                var user = await _userService.Login(model);
                if(user!= null)
                {
                    // create jwt authentication token and send to client
                    return Json(new { Token = GetJWTToken(user.UserId) });
                }
               

                return Json(new { Error = "Invalid username or password" });
            }

            return Json(new { Error = "Please fill all the fields" });
         
        }

        private string GetJWTToken(int userId)
        {
            // create JWT token

            var tokenHandler = new JwtSecurityTokenHandler();


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, userId.ToString())
                }),

                //TODO: get it from config
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token =tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
