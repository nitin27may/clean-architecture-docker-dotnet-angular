using Contact.Domain.Entities;

namespace Contact.Application.Interfaces;

public interface IActivityLogService
{
    Task LogActivityAsync(ActivityLogEntry logEntry);
    Task<IEnumerable<ActivityLogEntry>> GetActivityLogsAsync(string username, string email);
}
