using Contact.Domain.Entities;

namespace Contact.Domain.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotifications(Guid userId);
        Task MarkAsRead(Guid userId, Guid notificationId);
    }
}
