using System.Net;
using System.Text.Json;

namespace ValueTech.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                if (ex is ArgumentException)
                {
                    _logger.LogWarning("Error de validación de negocio: {Message}", ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "Ocurrió una excepción no controlada: {Message}", ex.Message);
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            object response;

            switch (exception)
            {
                case ArgumentException argEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new { error = argEx.Message }; 
                    break;
                
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new
                    {
                        error = "Ha ocurrido un error interno en el servidor. Por favor contacte al administrador.",
                        detail = exception.Message // En producción, esto debería ocultarse o ser genérico.
                    };
                    break;
            }

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
