using CoffeeTime.Models;
using CoffeeTime.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTime.Data
{
    public class CoffeeTimeDbContext : IdentityDbContext<ApplicationUser>
    {
        public CoffeeTimeDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Order> Order { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<MenuList> MenuList { get; set; }
    }
}