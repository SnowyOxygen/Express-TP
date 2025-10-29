using System.ComponentModel.DataAnnotations;

namespace Business.Product.CreateProductUseCase
{
    public sealed class CreateProductRequest
    {
        [Range(0, 30000)]
        public required int Price { get; set; }

        [Range(0, 30000)]
        public required int SalePrice { get; set; }

        [MinLength(2), MaxLength(100)]
        public required string Title { get; set; }

        [MinLength(2), MaxLength(500)]
        public required string Description { get; set; }

        [Range(1, int.MaxValue)]
        public required int Stock { get; set; }

        public required int CategoryId { get; set; }
    }
}
