using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySite.Data;
using MySite.Models;
using Newtonsoft.Json;
//using MySite.Migrations;

namespace MySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private DbUserContex _userContext;
        //private DbUserContex _uC;
        private SignInManager<Users> _signInManager;
        private UserManager<Users> _userManager;
        //private RoleManager<Users> _roleManager;
        private IConfiguration _configuration;
        //private 

        public UserController(
            DbUserContex userContext,
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            //RoleManager<Users> roleManager,
            IConfiguration configuration
            )
        {
            _userContext = userContext;
            _userManager = userManager;
            _signInManager = signInManager;
            //_roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Users>> Get()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<object>> Register([FromBody] Users model)
        {
            var user = new Users
            {
                UserName = model.UserName,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.PasswordHash);

            if (result.Succeeded)
            {
                //await _userContext.SaveChangesAsync();
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(model.Email, user);
            }

            return BadRequest(result.Errors);
            //throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Object>> Login([FromBody] Users model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.PasswordHash, false /* remember me*/, false);

            if (result.Succeeded)
            {
                //await _userContext.SaveChangesAsync();
                var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.UserName);
                return await GenerateJwtToken(appUser.Email, appUser);
            }

            //throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
            return BadRequest(ModelState.IsValid);
        }

        private async Task<object> GenerateJwtToken(string email, Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}