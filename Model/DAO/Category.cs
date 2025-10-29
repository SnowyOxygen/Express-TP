using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Model.DAO
{
    public sealed class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(100), MinLength(2)]
        public required string Title { get; set; }

        [MaxLength(500), MinLength(2)]
        public required string Description { get; set; }

        [MaxLength(7), MinLength(7)]
        public required string Color { get; set; }
    }
}
