using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces;


public interface IActivityLogRepository
{
    Task LogActivityAsync(ActivityLogEntry logEntry);
}
