using LearningManagementSystem.API.Hubs.ClientsInterfaces;
using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.API.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly AppDbContext _db;
        private string User = "user";

        public ChatHub(ILogger<ChatHub> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public Task MessageHandler(ChatMessage message)
        {
            message.Date = DateTime.Now;
            var s =Context.Items.TryGetValue(User, out var sender );
            message.Sender = ((Student) sender).User.UserName;
            return Clients.Others.Send(message);
        }

        public async Task<ChatServerResponse> Handshake(string userId)
        {
            var user = await _db.Students
                .Include(i => i.User)
                .FirstOrDefaultAsync(f => f.Id.Equals(Guid.Parse(userId)));
            Groups.
            if (user is null)
            {
                _logger.LogCritical("Wrong user!");

                return new ChatServerResponse()
                {
                    IsSuccessful = false,
                    Message = "Wrong user!"
                };

            }
            Context.Items.TryAdd(User, user);
            _logger.LogInformation("Handshake invocation!");

            return new ChatServerResponse()
            {
                IsSuccessful = true,
                Message = "You has been successfully connected"
            };
        }

        public override Task OnConnectedAsync()
        {
            
            _logger.LogInformation(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}