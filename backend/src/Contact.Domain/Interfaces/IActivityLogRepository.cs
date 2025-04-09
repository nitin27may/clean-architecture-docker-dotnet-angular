using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces;

public interface IActivityLogRepository
{
    Task LogActivityAsync(ActivityLogEntry logEntry);
    Task<IEnumerable<ActivityLogEntry>> GetActivityLogsAsync(string username, string email);
}
