using Microsoft.AspNetCore.Identity;
using MySql.Data.EntityFrameworkCore.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySite.Models
{
    public class Users : IdentityUser
    {
        [MySqlCharset("utf-8")]
        public string FirstNmae { get; set; }
        [MySqlCharset("utf-8")]
        public string LastName { get; set; }
    }
}
