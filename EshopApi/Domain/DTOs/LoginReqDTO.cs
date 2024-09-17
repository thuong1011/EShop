namespace EshopApi.Domain.DTOs
{
    public class LoginReqDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
