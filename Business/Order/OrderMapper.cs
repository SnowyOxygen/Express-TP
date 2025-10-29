namespace Business.Order
{
    internal static class OrderMapper
    {
        internal static OrderDto ToDto(this Model.DAO.Order order, List<Model.DAO.Product> products)
        {
            long totalPrice = order.OrderLines?.Sum(ol => ol.UnitPrice * ol.Quantity) ?? 0;
            OrderStatus status = OrderStatus.Pending;
            if (order.PaidTimestamp.HasValue) status = OrderStatus.Paid;
            if (order.CanceledTimestamp.HasValue) status = OrderStatus.Canceled;

            return new OrderDto
            {
                Id = order.Id,
                CreationTimestamp = order.CreationTimestamp,
                PaidTimestamp = order.PaidTimestamp,
                CanceledTimestamp = order.CanceledTimestamp,
                TotalPrice = totalPrice,
                Status = status,
                OrderLines = order.OrderLines?.ToDtoList(products) ?? []
            };
        }

        internal static List<OrderLineDto> ToDtoList(this IEnumerable<Model.DAO.OrderLine> orderLines, List<Model.DAO.Product> products)
        {
            List<OrderLineDto> orderLineDtos = new List<OrderLineDto>();

            foreach (var orderLine in orderLines)
            {
                Model.DAO.Product product = products.First(p => p.Id == orderLine.ProductId);

                if (product != null)
                {
                    OrderLineDto orderLineDto = new ()
                    {
                        ProductId = orderLine.ProductId,
                        ProductCategory = product?.Category?.Title ?? "none",
                        ProductDescription = product?.Description ?? "",
                        ProductName = product?.Title ?? "",
                        ChosenPrice = orderLine.UnitPrice,
                        Quantity = orderLine.Quantity
                    };

                    orderLineDtos.Add(orderLineDto);
                }
            }
            return orderLineDtos;
        }
    }
}
