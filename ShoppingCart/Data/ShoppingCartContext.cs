using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext (DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        public DbSet<ShoppingCart.Models.User> User { get; set; } = default!;
        public DbSet<ShoppingCart.Models.Product> Product { get; set; } = default!;
        public DbSet<ShoppingCart.Models.ProductImage> ProductImage { get; set; } = default!;
    }
}
