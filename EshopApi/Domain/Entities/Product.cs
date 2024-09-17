using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EshopApi.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [Range(0, long.MaxValue)]
        public required long Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
