using EShopping.Areas.UI.Controllers;
using EShopping.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EShopping.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
		public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Company> Company { get; set; }
		public DbSet<Cart> Cart { get; set; }
		public DbSet<Order> Order { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
	}
}