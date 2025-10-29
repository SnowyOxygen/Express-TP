using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DAO
{
    public sealed class OrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required long OrderId { get; set; }
        public required long ProductId { get; set; }
        public required int UnitPrice { get; set; }
        public required int Quantity { get; set; }

        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
