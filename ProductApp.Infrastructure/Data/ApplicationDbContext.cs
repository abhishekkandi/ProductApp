using Microsoft.EntityFrameworkCore;
using ProductApp.Infrastructure.Data.Configurations;
 
namespace ProductApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
 
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships and constraints here
        }
    }
}
 