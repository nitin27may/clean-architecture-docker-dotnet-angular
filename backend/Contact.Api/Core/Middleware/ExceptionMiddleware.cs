using Contact.Domain.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace Contact.Api.Core.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);  // Process the request
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
            ValidationException => StatusCodes.Status400BadRequest,
            BusinessException => StatusCodes.Status400BadRequest,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            NotAuthenticatedException => StatusCodes.Status401Unauthorized,
            ArgumentOutOfRangeException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        if (exception is ValidationException validationException)
        {
            // Invalid input is a client error, not a server fault: log it as a warning.
            logger.LogWarning("Request validation failed: {Failures}", validationException.Message);
            return WriteValidationErrorAsync(context, validationException);
        }

        logger.LogError(exception, exception.Message);
        
        // Handle JWT cryptography errors specifically
        string errorMessage = exception.Message;
        if (exception is ArgumentOutOfRangeException && 
            errorMessage.Contains("KeyedHashAlgorithm") && 
            errorMessage.Contains("key size"))
        {
            errorMessage = "Authentication error: Invalid JWT signing key configuration. Please contact system administrators.";
        }

        var result = JsonSerializer.Serialize(new
        {
            error = errorMessage,
            // The Angular error interceptor displays error.message
            message = errorMessage,
            technicalDetails = exception is ApplicationException ? null : exception.GetType().Name
        });

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }

    private static Task WriteValidationErrorAsync(HttpContext context, ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).Distinct().ToArray());

        var result = JsonSerializer.Serialize(new
        {
            error = "One or more validation errors occurred.",
            message = string.Join("; ", errors.Values.SelectMany(messages => messages)),
            errors,
        });

        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return context.Response.WriteAsync(result);
    }
}
