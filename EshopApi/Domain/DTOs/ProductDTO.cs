namespace EshopApi.Domain.DTOs
{
    public class ProductDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required long Price { get; set; }
        public string? Description { get; set; }
    }
}
