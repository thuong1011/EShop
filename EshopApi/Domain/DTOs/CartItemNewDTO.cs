namespace EshopApi.Domain.DTOs
{
    public class CartItemNewDTO
    {
        public required int UserId { get; set; }
        public required int ProductId { get; set; }
    }
}
