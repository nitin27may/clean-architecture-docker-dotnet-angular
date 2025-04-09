using Contact.Application.Interfaces;
using Contact.Application.UseCases.Notifications;
using Contact.Domain.Entities;
using Contact.Domain.Interfaces;

namespace Contact.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationResponse>> GetUserNotifications(Guid userId)
        {
            var notifications = await _notificationRepository.GetUserNotifications(userId);
            return notifications.Select(n => new NotificationResponse
            {
                Id = n.Id,
                UserId = n.UserId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedOn = n.CreatedOn,
                CreatedBy = n.CreatedBy,
                UpdatedOn = n.UpdatedOn,
                UpdatedBy = n.UpdatedBy
            });
        }

        public async Task MarkAsRead(Guid userId, Guid notificationId)
        {
            var notification = await _notificationRepository.GetById(notificationId);
            if (notification == null || notification.UserId != userId)
            {
                throw new Exception("Notification not found or access denied.");
            }

            notification.IsRead = true;
            await _notificationRepository.Update(notification);
        }
    }
}
