using Business.Category;

namespace Business.Product
{
    public sealed class ProductDto
    {
        public required long Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Price { get; set; }
        public required int SalePrice { get; set; }
        public required int Stock { get; set; }

        public required CategoryDto? Category { get; set; }
    }
}
