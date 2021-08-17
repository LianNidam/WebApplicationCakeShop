using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplicationCakeShop.Models;

namespace WebApplicationCakeShop.Data
{
    public class WebApplicationCakeShopContext : DbContext
    {
        public WebApplicationCakeShopContext (DbContextOptions<WebApplicationCakeShopContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplicationCakeShop.Models.Cake> Cake { get; set; }

        public DbSet<WebApplicationCakeShop.Models.Cart> Cart { get; set; }

        public DbSet<WebApplicationCakeShop.Models.Category> Category { get; set; }

        public DbSet<WebApplicationCakeShop.Models.User> User { get; set; }
    }
}
