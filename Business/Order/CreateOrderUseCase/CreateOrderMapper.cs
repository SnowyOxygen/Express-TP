using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Order.CreateOrderUseCase
{
    internal static class CreateOrderMapper
    {
        internal static Model.DAO.Order ToDao(this CreateOrderRequest request)
        {
            long creationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            return new Model.DAO.Order
            {
                CreationTimestamp = creationTimestamp,
            };
        }

        internal static Model.DAO.OrderLine ToDao(
            this CreateOrderLine createOrderLine,
            long orderId,
            int unitPrice)
        {
            return new Model.DAO.OrderLine
            {
                OrderId = orderId,
                UnitPrice = unitPrice,
                ProductId = createOrderLine.ProductId,
                Quantity = createOrderLine.Quantity,
            };
        }
    }
}
