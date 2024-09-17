namespace EshopApi.Domain.DTOs
{
    public class ProductNewDTO
    {
        public required string Name { get; set; }
        public required long Price { get; set; }
        public string? Description { get; set; }
    }
}
