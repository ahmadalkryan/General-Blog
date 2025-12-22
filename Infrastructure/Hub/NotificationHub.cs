//using Microsoft.AspNetCore.SignalR;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure
//{
//    public class NotificationHub: Hub
//    {
//        private readonly ILogger<NotificationHub> _logger;

//        public NotificationHub(ILogger<NotificationHub> logger)
//        {
//            _logger = logger;
//        }


//        public async Task BroadcastToAllAsync(string eventName, object data)
//        {
//            try
//            {
//                await Clients.All.SendAsync(eventName, data);
//                _logger.LogDebug("Broadcasted {EventName} to all clients", eventName);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error broadcasting {EventName}", eventName);
//            }
//        }

//        public override async Task OnConnectedAsync()
//        {
//            _logger.LogInformation("🔗 Client connected: {ConnectionId}", Context.ConnectionId);
//            await base.OnConnectedAsync();
//        }

//        public override async Task OnDisconnectedAsync(Exception? exception)
//        {
//            _logger.LogInformation("🔗 Client disconnected: {ConnectionId}", Context.ConnectionId);
//            await base.OnDisconnectedAsync(exception);
//        }



//    }
//}
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private static readonly Dictionary<string, string> _userConnections = new();

        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }

        private string GetUserId()
        {
            return Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[Context.ConnectionId] = userId;
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");

                _logger.LogInformation("✅ User {UserId} connected", userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _userConnections.Remove(Context.ConnectionId);
            var userId = GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}