using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.DataContext.Migrations
{
    internal static class CategoryMigration
    {
        internal static void MigrateCategories(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(c =>
            {
                c.HasIndex(c => c.Title).IsUnique();
            });

            modelBuilder.Entity<Category>().HasData(
                new Category {
                    Id = 1,
                    Title = "Electronics",
                    Description = "Discover the latest gadgets, devices, and electronic accessories",
                    Color = "#00BCD4"
                },
                new Category {
                    Id = 2,
                    Title = "Books",
                    Description = "Explore a vast collection of books across all genres",
                    Color = "#4CAF50"
                },
                new Category {
                    Id = 3,
                    Title = "Clothing",
                    Description = "Find trendy and comfortable clothing for all occasions",
                    Color = "#FF5722"
                },
                new Category {
                    Id = 4,
                    Title = "Home & Kitchen",
                    Description = "Everything you need to make your house a home",
                    Color = "#9C27B0"
                },
                new Category {
                    Id = 5,
                    Title = "Sports & Outdoors",
                    Description = "Equipment and gear for sports and outdoor activities",
                    Color = "#2196F3"
                }
            );
        }
    }
}
