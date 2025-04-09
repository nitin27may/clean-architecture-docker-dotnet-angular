using Contact.Domain.Exceptions;
using System.Text.Json;

namespace Contact.Api.Core.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);  // Process the request
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            BusinessException => StatusCodes.Status400BadRequest,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            NotAuthenticatedException => StatusCodes.Status401Unauthorized,
            ArgumentOutOfRangeException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        _logger.LogError(exception, exception.Message);
        
        // Handle JWT cryptography errors specifically
        string errorMessage = exception.Message;
        if (exception is ArgumentOutOfRangeException argEx && 
            errorMessage.Contains("KeyedHashAlgorithm") && 
            errorMessage.Contains("key size"))
        {
            errorMessage = "Authentication error: Invalid JWT signing key configuration. Please contact system administrators.";
        }

        var result = JsonSerializer.Serialize(new
        {
            error = errorMessage,
            technicalDetails = exception is ApplicationException ? null : exception.GetType().Name
        });

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}
