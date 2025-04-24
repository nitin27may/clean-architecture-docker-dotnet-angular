using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Domain.Entities;

namespace Contact.Api.Core.Middleware;

public class ActivityLoggingMiddleware(RequestDelegate next, ILogger<ActivityLoggingMiddleware> logger, IServiceProvider serviceProvider)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var activityAttribute = endpoint?.Metadata.GetMetadata<ActivityLogAttribute>();

        if (activityAttribute is not null)
        {
            using var scope = serviceProvider.CreateScope();
            var activityLogService = scope.ServiceProvider.GetRequiredService<IActivityLogService>();
            
            // Safely extract claims from User context
            var userIdClaim = context.User.FindFirst("Id");
            var userId = userIdClaim is not null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
            
            var username = context.User.Identity?.Name ?? "Anonymous";
            
            // Safely extract email claim
            var emailClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.Email) 
                ?? context.User.FindFirst("Email");
            var email = emailClaim?.Value ?? "unknown@example.com";
            
            var logEntry = new ActivityLogEntry
            {
                UserId = userId,
                Username = username,
                Email = email,
                Activity = activityAttribute.ActivityDescription,
                Endpoint = context.Request.Path,
                HttpMethod = context.Request.Method,
                Timestamp = DateTimeOffset.UtcNow,
                IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                UserAgent = context.Request.Headers["User-Agent"].ToString()
            };

            await activityLogService.LogActivityAsync(logEntry);
        }

        await next(context);
    }
}
