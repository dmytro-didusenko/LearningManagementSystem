using Microsoft.AspNetCore.SignalR;

namespace LearningManagementSystem.API.Hubs
{
    public class ChatHub: Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public async Task MessageHandler(string message)
        {
            _logger.LogCritical($"{message}");
            await Clients.Others.SendAsync("Send", message);
        }
    }
}
