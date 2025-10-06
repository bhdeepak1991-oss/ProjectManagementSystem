using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace PMS.Notification
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

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

            // Optionally: echo message back to sender
            await Clients.Caller.SendAsync("ReceiveMessage", senderName, message);
        }

    }
}
