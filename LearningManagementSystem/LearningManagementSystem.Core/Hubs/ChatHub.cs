using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.Sorting;

namespace LearningManagementSystem.API.Hubs
{
    //TODO: Make managing many connections of one user
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly AppDbContext _db;

        public ChatHub(ILogger<ChatHub> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task MessageHandler(ChatMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                return;
            }

            var sender = Context.Items["User"] as Student;

            message.Sender = sender.User.UserName;

            await _db.GroupChatMessages.AddAsync(new GroupChatMessage()
            {
                SenderId = sender.Id,
                GroupId = sender.Group.Id,
                Text = message.Text,
                CreationDate = message.Date
            });
            await _db.SaveChangesAsync();
            var group = Context.Items["Group"] as Group;
            await Clients.OthersInGroup(group.Name).SendAsync("Send", message);
        }

        public async Task Handshake(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedId))
            {
                await CloseClientConnectionAsync("Wrong user data");
            }

            var user = await _db.Students
                .Include(i => i.Group)
                    .ThenInclude(t => t.ChatMessages)
                    .ThenInclude(t => t.Sender)
                .Include(i => i.User)
                .FirstOrDefaultAsync(f => f.Id.Equals(parsedId));

            if (user is null || user.Group is null)
            {
                await CloseClientConnectionAsync("Wrong user data");
            }

            var group = user!.Group;
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
            Context.Items.TryAdd("User", user);
            Context.Items.TryAdd("Group", group);
        }

        public Task<ChatHistory> GetChatHistory()
        {
            var group = Context.Items["Group"] as Group;
            var user = Context.Items["User"] as Student;
            var chatMessages = group.ChatMessages.Select(m => new ChatMessage()
            {
                Sender = m.Sender.UserName.Equals(user.User.UserName) ? "Me" : m.Sender.UserName,
                Date = m.CreationDate,
                Text = m.Text
            }).ToList();

            var chatHistory = new ChatHistory()
            {
                GroupId = group.Id,
                GroupName = group.Name,
                ChatMessages = chatMessages
            };

            return Task.FromResult(chatHistory);
        }

        //TODO: Rewrite in more 'friendly' form
        private async Task CloseClientConnectionAsync(string errorMessage)
        {
            await Clients.Caller.SendAsync("Disconnect", new ChatServerResponse()
            {
                IsSuccessful = false,
                Message = errorMessage
            });
            Context.Abort();
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"New connection with id: [{Context.ConnectionId}]");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Connection id: [{Context.ConnectionId}] is disconnected");
            return base.OnDisconnectedAsync(exception);
        }
    }
}