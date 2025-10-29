using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DAO
{
    public sealed class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public required long CreationTimestamp { get; set; }
        public long? PaidTimestamp { get; set; }
        public long? CanceledTimestamp { get; set; }

        public List<OrderLine>? OrderLines { get; set; }
    }
}
