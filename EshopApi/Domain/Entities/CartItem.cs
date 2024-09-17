using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EshopApi.Domain.Entities
{
    [Index(nameof(UserId), nameof(ProductId), IsUnique = true)]
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required int UserId { get; set; }

        [Required]
        public required int ProductId { get; set; }
    }
}
