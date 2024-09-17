namespace EshopApi.Domain.DTOs
{
    public class ResponseWrapperDTO<T>
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
