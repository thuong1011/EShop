using EshopApi.Domain.DTOs;

namespace EshopApi.Presentation.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Console.WriteLine($"Exception: {exception.Message}");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
            var response = new ResponseWrapperDTO<string>
            {
                Status = false,
                Message = exception.Message,
                Data = exception.ToString()
            };
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
