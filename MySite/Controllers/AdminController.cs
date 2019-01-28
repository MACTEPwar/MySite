using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MySite.Data;
using MySite.Models;
//using MySite.Migrations;

namespace MySite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private DbUserContex _userContext;
        //private DbUserContex _uC;
        private UserManager<Users> _userManager;
        //private 

        public AdminController(DbUserContex userContext, UserManager<Users> userManager)
        {
            _userContext = userContext;
            _userManager = userManager;
        }

        //// api/admin/{password}
        //[HttpGet("{pass}")]
        //public async Task<Object> Get(string pass)
        //{
        //    IdentityResult result = null;
        //    var user = new Users { UserName = "MACTEPwar", Email = "shebanic95@gmail.com" };
        //    result = await _userManager.CreateAsync(user, pass);
        //    if (result.Succeeded)
        //    {
        //        await _userContext.SaveChangesAsync();
        //        return result;
        //    }
        //    return result;
        //}


    }
}