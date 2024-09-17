namespace EshopApi.Domain.DTOs
{
    public class UpdatePasswordReqDTO
    {
        public required int UserId { get; set; }
        public required string Password { get; set; }
    }
}
