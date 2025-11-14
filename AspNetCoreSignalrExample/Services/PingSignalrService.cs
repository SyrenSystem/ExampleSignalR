using Microsoft.AspNetCore.SignalR;
using AspNetCoreSignalrExample.Hubs;

namespace AspNetCoreSignalrExample.Services
{
    public class PingSignalrService : BackgroundService
    {
        private readonly IHubContext<PingHub> _hubContext;
        private readonly ILogger<PingSignalrService> _logger;
        private readonly TimeSpan _pingInterval = TimeSpan.FromSeconds(5);

        public PingSignalrService(
            IHubContext<PingHub> hubContext,
            ILogger<PingSignalrService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ping SignalR Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var pingMessage = new
                    {
                        Timestamp = DateTime.UtcNow,
                        Message = "Ping from server"
                    };

                    // Send to all connected clients
                    await _hubContext.Clients.All.SendAsync("ReceivePing", pingMessage, stoppingToken);

                    _logger.LogDebug("Ping sent at {Time}", pingMessage.Timestamp);

                    await Task.Delay(_pingInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Expected when stopping
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending ping");
                }
            }

            _logger.LogInformation("Ping SignalR Service stopped");
        }
    }
}
