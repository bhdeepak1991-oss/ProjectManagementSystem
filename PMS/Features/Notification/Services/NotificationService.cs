using Microsoft.AspNetCore.SignalR;
using PMS.Domains;
using PMS.Features.Notification.Repositories;
using PMS.Notification;

namespace PMS.Features.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }

        public async  Task<(string message, bool isSuccess)> AddNotification(NotificationDetail model, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync(model.NotificationMessage ?? string.Empty, model.Id, model.NotificationStatus);

            return await _notificationRepository.AddNotification(model, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> ChangeStatus(int notificationId, string status, CancellationToken cancellationToken)
        {
            return await _notificationRepository.ChangeStatus(notificationId, status, cancellationToken);
        }

        public async Task<(string message, bool isSuccess)> CreateMessage(ProjectMessage model)
        {
            return await _notificationRepository.CreateMessage(model);
        }

        public async Task<(string messsage, bool isSuccess, IEnumerable<ProjectMessage> models)> GetMessage(string userName)
        {
            return await _notificationRepository.GetMessage(userName);
        }

        public async  Task<(string message, bool isSuccess, IEnumerable<NotificationDetail> models)> GetNotificationList(int userId, CancellationToken cancellationToken)
        {
            return await _notificationRepository.GetNotificationList(userId, cancellationToken);
        }
    }
}
