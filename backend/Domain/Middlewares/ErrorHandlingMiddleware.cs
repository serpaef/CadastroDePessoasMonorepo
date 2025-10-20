using backend.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace backend.Domain.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string title;
            string detail = exception.Message;
            object? errors = null;

            switch (exception)
            {
                case BadRequestException badRequest:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    break;

                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Not Found";
                    break;

                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Unauthorized";
                    break;

                case ForbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    title = "Forbidden";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    title = "Internal Server Error";
                    _logger.LogError(exception, "Erro inesperado");
                    break;
            }

            var problemDetails = new
            {
                status = (int)statusCode,
                title,
                detail,
                errors
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonOptions));
        }
    }
}
