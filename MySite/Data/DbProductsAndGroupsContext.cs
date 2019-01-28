using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySite.Models;
using System.Data;

namespace MySite.Data
{
    public class DbProductsAndGroupsContext : DbContext
    {
        public DbProductsAndGroupsContext(DbContextOptions<DbProductsAndGroupsContext> option) 
            : base (option)
        {

        }

        public DbSet<Products> products { get; set; }
        public DbSet<Groups> groups { get; set; }
        public DbSet<GropsAndProducts> gropsAndProducts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}
