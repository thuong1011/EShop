namespace EshopApi.Domain.DTOs
{
    public class CartItemDTO
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public required int ProductId { get; set; }
    }
}
