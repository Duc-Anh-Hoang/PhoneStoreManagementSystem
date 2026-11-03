using System.Net;
using System.Text.Json;
using FluentValidation;
using PhoneStoreAPI.Common.Exceptions;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;
        List<string>? errors = null;

        switch (exception)
        {
            case ValidationException valEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Dữ liệu không hợp lệ.";
                errors = valEx.Errors.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Validation failure [{TraceId}]: {Message} - Errors: {@Errors}", 
                    context.TraceIdentifier, message, errors);
                break;

            case AppException appEx:
                statusCode = (int)appEx.StatusCode;
                message = appEx.Message;
                errors = appEx.Errors;
                _logger.LogWarning("Application error [{TraceId}] (Status {StatusCode}): {Message}", 
                    context.TraceIdentifier, statusCode, message);
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Đã xảy ra lỗi hệ thống. Vui lòng thử lại sau.";
                _logger.LogError(exception, "Unhandled server error [{TraceId}]: {Message}", 
                    context.TraceIdentifier, exception.Message);
                break;
        }

        context.Response.StatusCode = statusCode;

        var responsePayload = new
        {
            message,
            errors,
            traceId = context.TraceIdentifier
        };

        var options = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(responsePayload, options));
    }
}
