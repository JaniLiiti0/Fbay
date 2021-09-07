using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fbay.Models;

namespace Fbay.Data
{
    public class FbayContext : DbContext
    {
        public FbayContext (DbContextOptions<FbayContext> options)
            : base(options)
        {
        }

        public DbSet<Fbay.Models.Listings> Listings { get; set; }
        public DbSet<Fbay.Models.Users> Users { get; set; }

        public DbSet<Fbay.Models.Orders> Orders { get; set; }
        //public DbSet<Fbay.Models.Cart> Cart { get; set; }

        //public DbSet<Fbay.Models.Product> Product { get; set; }
    }
}
