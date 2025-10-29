namespace Business.Order.CreateOrderUseCase
{
    public class CreateOrderRequest
    {
        public required IEnumerable<CreateOrderLine> OrderLines { get; set; }
    }
}
