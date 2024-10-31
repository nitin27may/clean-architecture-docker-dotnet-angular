using Contact.Api.Core.Attributes;
using Contact.Application.Interfaces;
using Contact.Domain.Entities;

namespace Contact.Api.Core.Middleware;

public class ActivityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ActivityLoggingMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ActivityLoggingMiddleware(RequestDelegate next, ILogger<ActivityLoggingMiddleware> logger, IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var activityAttribute = endpoint?.Metadata.GetMetadata<ActivityLogAttribute>();

        if (activityAttribute != null)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var activityLogService = scope.ServiceProvider.GetRequiredService<IActivityLogService>();
                var userIdClaim = context.User.FindFirst("id");
                var userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
                var activityDescription = activityAttribute.ActivityDescription;
                var endpointPath = context.Request.Path;
                var httpMethod = context.Request.Method;
                var ipAddress = context.Connection.RemoteIpAddress?.ToString();
                var userAgent = context.Request.Headers["User-Agent"].ToString();

                var logEntry = new ActivityLogEntry
                {
                    UserId = userId,
                    Activity = activityDescription,
                    Endpoint = endpointPath,
                    HttpMethod = httpMethod,
                    Timestamp = DateTimeOffset.UtcNow,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };

                await activityLogService.LogActivityAsync(logEntry);
            }
        }

        await _next(context);
    }
}
