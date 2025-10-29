using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.DataContext.Migrations
{
    internal static class ProductMigration
    {
        internal static void MigrateProducts(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            Product testProduct1 = new()
            {
                Id = 1,
                Title = "Fridge",
                Description = "For cooling stuff",
                Price = 10000,
                CategoryId = 1,
                SaleActive = false,
                SalePrice = 8000,
                Stock = 50
            };

            Product testProduct2 = new()
            {
                Id = 2,
                Title = "Laptop",
                Description = "For working on the go",
                Price = 150000,
                CategoryId = 2,
                SaleActive = true,
                SalePrice = 120000,
                Stock = 2
            };

            modelBuilder.Entity<Product>().HasData(
                testProduct1,
                testProduct2
            );
        }
    }
}
