using Contact.Application.UseCases.Notifications;

namespace Contact.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationResponse>> GetUserNotifications(Guid userId);
        Task MarkAsRead(Guid userId, Guid notificationId);
    }
}
