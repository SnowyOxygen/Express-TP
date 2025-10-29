using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DAO
{
    public sealed class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required long CategoryId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int Price { get; set; }
        public required int SalePrice { get; set; }
        public required bool SaleActive { get; set; } = false;
        public required int Stock { get; set; }

        public Category? Category { get; set; }
    }
}
