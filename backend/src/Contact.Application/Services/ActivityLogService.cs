using Contact.Application.Interfaces;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Contact.Application.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly ILogger<ActivityLogService> _logger;
    public ActivityLogService(IActivityLogRepository activityLogRepository, ILogger<ActivityLogService> logger)
    {
        _activityLogRepository = activityLogRepository;
        _logger = logger;
    }

    public async Task LogActivityAsync(ActivityLogEntry logEntry)
    {
        await _activityLogRepository.LogActivityAsync(logEntry);
    }

    public async Task<IEnumerable<ActivityLogEntry>> GetActivityLogsAsync(string username, string email)
    {
        return await _activityLogRepository.GetActivityLogsAsync(username, email);
    }
}
