namespace EshopApi.Domain.DTOs
{
    public class UserDTO
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}
