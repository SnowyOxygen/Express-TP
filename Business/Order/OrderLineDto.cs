namespace Business.Order
{
    public class OrderLineDto
    {
        public required long ProductId { get; set; }
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
        public required string ProductCategory { get; set; }
        public required int Quantity { get; set; }
        public required int ChosenPrice { get; set; }
    }
}
