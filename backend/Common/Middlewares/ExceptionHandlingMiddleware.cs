using System.Net;
using System.Text.Json;
using PhoneStoreAPI.Common.Exceptions;
using PhoneStoreAPI.Common.Responses;

namespace PhoneStoreAPI.Common.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            AppException appEx => (int)appEx.StatusCode,
            _ => (int)HttpStatusCode.InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var response = ApiResponse<object>.Fail(
            statusCode == (int)HttpStatusCode.InternalServerError 
                ? "Đã xảy ra lỗi máy chủ nội bộ. Vui lòng thử lại sau." 
                : exception.Message
        );

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
