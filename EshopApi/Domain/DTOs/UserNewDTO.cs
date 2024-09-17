namespace EshopApi.Domain.DTOs
{
    public class UserNewDTO
    {
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Password { get; set; }
    }
}
