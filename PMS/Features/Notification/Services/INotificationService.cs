using PMS.Domains;

namespace PMS.Features.Notification.Services
{
    public interface INotificationService
    {
        Task<(string message, bool isSuccess)> AddNotification(NotificationDetail model, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess)> ChangeStatus(int notificationId, string status, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess, IEnumerable<NotificationDetail> models)> GetNotificationList(int userId, CancellationToken cancellationToken);

        Task<(string message, bool isSuccess)> CreateMessage(ProjectMessage model);

        Task<(string messsage, bool isSuccess, IEnumerable<ProjectMessage> models)> GetMessage(string userName);






    }
}
