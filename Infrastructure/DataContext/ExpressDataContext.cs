using Infrastructure.DataContext.Migrations;
using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.DataContext
{
    public sealed class ExpressDataContext(DbContextOptions<ExpressDataContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

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
