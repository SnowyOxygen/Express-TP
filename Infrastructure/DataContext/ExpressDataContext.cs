using Infrastructure.DataContext.Migrations;
using Microsoft.EntityFrameworkCore;
using Model.DAO;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.DataContext
{
    public sealed class ExpressDataContext(DbContextOptions<ExpressDataContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            // Ignore transaction warnings for in-memory database
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.MigrateProducts();
            modelBuilder.MigrateOrders();
            modelBuilder.MigrateCategories();
            modelBuilder.MigrateOrderLines();
        }
    }
}
