using Microsoft.AspNetCore.SignalR;
using PMS.Features.Notification.Services;
using PMS.Features.Project.Services;
using System.Collections.Concurrent;
using PMS.Helpers;

namespace PMS.Notification
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub(INotificationService notificationService, IHttpContextAccessor contextAccessor)
        {
            _notificationService = notificationService;
            _httpContextAccessor= contextAccessor;
        }

        public override Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext()?.Request.Query["userId"]!;
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context.ConnectionId;
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var item = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId);
            if (!string.IsNullOrEmpty(item.Key))
            {
                _userConnections.TryRemove(item.Key, out _);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToUser(string receiverUserId, string message, string senderName)
        {
            if (_userConnections.TryGetValue(receiverUserId, out string connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderName, message);
            }

            await _notificationService.CreateMessage(new Domains.ProjectMessage
            {
                FromMessage = senderName,
                Reciever = receiverUserId,
                MessageInfo = message,
                MessageStatus="New"
            });

            // Optionally: echo message back to sender
            await Clients.Caller.SendAsync("ReceiveMessage", senderName, message);
        }

    }
}
