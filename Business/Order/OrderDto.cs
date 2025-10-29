namespace Business.Order
{
    public class OrderDto
    {
        public required long Id { get; set; }
        public required long CreationTimestamp { get; set; }
        public long? PaidTimestamp { get; set; }
        public long? CanceledTimestamp { get; set; }
        public required long TotalPrice { get; set; }

        public required List<OrderLineDto> OrderLines { get; set; }

        public OrderStatus Status { get; set; }
    }
}
