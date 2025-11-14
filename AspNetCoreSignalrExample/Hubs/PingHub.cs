using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSignalrExample.Hubs
{
    public class PingHub : Hub
    {
        private readonly ILogger<PingHub> _logger;

        public PingHub(ILogger<PingHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        // Used by frontend to join specific groups
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation("Client {ConnectionId} joined group: {GroupName}", Context.ConnectionId, groupName);
        }

        // Used by frontend to leave specific groups
        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            _logger.LogInformation("Client {ConnectionId} left group: {GroupName}", Context.ConnectionId, groupName);
        }

        public async Task PingFromFrontend(string message)
        {
            var pingMessage = new
            {
                timestamp = DateTime.UtcNow.ToString("o"),
                message = message
            };

            await Clients.All.SendAsync("ReceivePing", pingMessage);
            _logger.LogInformation("Ping sent from {ConnectionId}: {Message}", Context.ConnectionId, message);
        }
    }
}
