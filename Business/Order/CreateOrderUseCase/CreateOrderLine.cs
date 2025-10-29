namespace Business.Order.CreateOrderUseCase
{
    public class CreateOrderLine
    {
        public required long ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}
