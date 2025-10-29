using System.ComponentModel.DataAnnotations;

namespace Business.Category.CreateCategoryUseCase
{
    public sealed class CreateCategoryRequest
    {
        [MinLength(2), MaxLength(100)]
        public required string Title { get; set; }

        [MinLength(2), MaxLength(500)]
        public required string Description { get; set; }

        [MinLength(7), MaxLength(7)]
        public required string Color { get; set; }
    }
}
