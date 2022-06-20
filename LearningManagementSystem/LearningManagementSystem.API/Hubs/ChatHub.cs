using System.Collections.Concurrent;
using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.API.Hubs
{
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
            message.Date = DateTime.Now;
            Context.Items.TryGetValue("User", out var sender);
            message.Sender = ((Student)sender).User.UserName;
            var senderUser = (Student)sender;

            await _db.GroupChatMessages.AddAsync(new GroupChatMessage()
            {
                SenderId = senderUser.Id,
                GroupId = senderUser.Group.Id,
                Text = message.Text
            });
            await _db.SaveChangesAsync();

            await Clients.Group(Context.Items["Group"] as string).SendAsync("Send", message);
        }

        public async Task<IEnumerable<ChatMessage>?> Handshake(string userId)
        {
            var user = await _db.Students
                .Include(i => i.Group)
                .ThenInclude(t => t.ChatMessages)
                .ThenInclude(t => t.Sender)
                .Include(i => i.User)
                .FirstOrDefaultAsync(f => f.Id.Equals(Guid.Parse(userId)));
            if (user is null)
            {
                _logger.LogCritical("Wrong user!");
                await Clients.Caller.SendAsync("Disconnect", new ChatServerResponse()
                {
                    IsSuccessful = false,
                    Message = "Wrong user data"
                });
                return null;
            }
            
            var group = user.Group;

            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);

            var chatHistory = group.ChatMessages.Select(m => new ChatMessage()
            {
                Sender = m.Sender.UserName,
                Date = m.CreationDate,
                Text = m.Text
            }).ToList();

            Context.Items.TryAdd("User", user);
            Context.Items.TryAdd("Group", group.Name);
            return chatHistory;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogCritical($"New connection to Hub: {Context.ConnectionId}" +
                                $"\nUser identifier: {Context.UserIdentifier}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogCritical($"Connection: {Context.ConnectionId} is disconnected");
            return base.OnDisconnectedAsync(exception);
        }
    }
}