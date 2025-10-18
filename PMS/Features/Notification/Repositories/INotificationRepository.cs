using PMS.Domains;

namespace PMS.Features.Notification.Repositories
{
    public interface INotificationRepository
    {
        Task<(string message, bool isSuccess)> AddNotification(NotificationDetail model, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess)> ChangeStatus(int notificationId, string status, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, IEnumerable<NotificationDetail> models)> GetNotificationList(int userId, CancellationToken cancellationToken);
    }
}
