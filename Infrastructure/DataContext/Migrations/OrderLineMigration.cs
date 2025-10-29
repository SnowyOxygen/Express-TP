using Microsoft.EntityFrameworkCore;
using Model.DAO;

namespace Infrastructure.DataContext.Migrations
{
    internal static class OrderLineMigration
    {
        internal static void MigrateOrderLines(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(ol => ol.OrderId);
            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Product)
                .WithMany()
                .HasForeignKey(ol => ol.ProductId);
        }
    }
}
