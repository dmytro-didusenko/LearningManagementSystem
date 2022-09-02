using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Hubs
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
            ArgumentNullException.ThrowIfNull(message);
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                return;
            }

            var user = GetCurrentUser();

            message.Sender = user.UserName;
            await _db.GroupChatMessages.AddAsync(new GroupChatMessage()
            {
                SenderId = user.UserId,
                GroupId = user.GroupId,
                Text = message.Text,
                CreationDate = message.Date
            });
            await _db.SaveChangesAsync();

            await Clients.Group(user.GroupName).SendAsync("ReceiveMessage", message);
        }

        public async Task Handshake(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedId))
            {
                await CloseClientConnectionAsync("Wrong user data");
            }

            var user = await _db.Students
                .Include(i => i.Group)
                .Include(i => i.User)
                .FirstOrDefaultAsync(f => f.Id.Equals(parsedId));

            if (user is null || user.Group is null)
            {
                await CloseClientConnectionAsync("Wrong user data");
                return;
            }

            var userModel = new ChatUserModel()
            {
                GroupId = user.Group.Id,
                GroupName = user.Group.Name,
                UserId = user.Id,
                UserName = user.User.UserName
            };
            var group = user!.Group;
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
            Context.Items.TryAdd("User", userModel);
        }

        public async Task<ChatUserModel> GetChatInfo()
        {
            var user = GetCurrentUser();

            var chatHistory = new ChatUserModel
            {
                UserName = user.UserName,
                UserId = user.UserId,
                GroupId = user.GroupId,
                GroupName = user.GroupName
            };

            return await Task.FromResult(chatHistory);
        }

        public async Task<ChatHistory> GetChatHistory()
        {
            var user = GetCurrentUser();

            var chatMessages = await _db.GroupChatMessages
                .Where(w => w.GroupId.Equals(user.GroupId))
                .Select(m => new ChatMessage()
                {
                    Sender = m.Sender.UserName,
                    Date = m.CreationDate,
                    Text = m.Text
                }).ToListAsync();

            var chatHistory = new ChatHistory()
            {
                UserName = user.UserName,
                UserId = user.UserId,
                GroupId = user.GroupId,
                GroupName = user.GroupName,
                ChatMessages = chatMessages
            };

            return chatHistory;
        }

        private async Task CloseClientConnectionAsync(string errorMessage)
        {
            await Clients.Caller.SendAsync("Disconnect", new ChatServerResponse()
            {
                IsSuccessful = false,
                Message = errorMessage
            });
        }

        private ChatUserModel GetCurrentUser()
        {
            var user = Context.Items["User"] as ChatUserModel;
            if (user is null)
            {
                throw new BadRequestException("No user data!");
            }

            return user;
        }
    }
}