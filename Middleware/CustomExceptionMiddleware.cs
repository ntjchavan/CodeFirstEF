using System.Text.Json;

namespace CodeFirstEFAPI.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred!");

                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)StatusCodes.Status500InternalServerError;

                var errorDetails = new
                {
                    message = ex.Message,
                    statusCode = response.StatusCode
                };

                var errorJson = JsonSerializer.Serialize(errorDetails);

                await response.WriteAsync(errorJson);

            }
        }

    }
}
