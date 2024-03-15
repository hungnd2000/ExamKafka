using Microsoft.EntityFrameworkCore;
using OrderApp.Data.EntityTypeConfigurations;
using OrderApp.Entities;

namespace OrderApp.Data
{
    public class OrderDbContext: DbContext 
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }   

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        }

    }
}
