using Microsoft.EntityFrameworkCore;
using OnlineShopNET.Domain.Entities;

namespace OnlineShopNET.Infrastructure.Data
{
    public class OnlineShopDbContext : DbContext
    {
        public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Product { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order2> Orders2 { get; set; }
    }
}
