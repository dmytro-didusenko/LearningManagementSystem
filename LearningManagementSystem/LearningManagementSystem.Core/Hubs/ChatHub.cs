using LearningManagementSystem.Core.Filters;
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
            message.Date = DateTime.Now;

            await _db.GroupChatMessages.AddAsync(new GroupChatMessage()
            {
                SenderId = sender.Id,
                GroupId = sender.Group.Id,
                Text = message.Text,
                CreationDate = message.Date
            });
            await _db.SaveChangesAsync();

            var group = Context.Items["Group"] as Group;

            await Clients.Group(group.Name).SendAsync("ReceiveMessage", message);
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

            var group = user!.Group;
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
            Context.Items.TryAdd("User", user);
            Context.Items.TryAdd("Group", group);
        }

        public async Task<GroupInfo> GetGroupInfo()
        {
            var group = Context.Items["Group"] as Group;
            var user = Context.Items["User"] as Student;

            var chatHistory = new GroupInfo
            {
                UserName = user.User.UserName,
                UserId = user.Id,
                GroupId = group.Id,
                GroupName = group.Name
            };

            return await Task.FromResult(chatHistory);
        }

        public async Task<ChatHistory> GetChatHistory()
        {
            var group = Context.Items["Group"] as Group;
            var user = Context.Items["User"] as Student;

            var chatMessages = await _db.GroupChatMessages
                .Where(w => w.GroupId.Equals(group.Id))
                .Select(m => new ChatMessage()
                {
                    Sender = m.Sender.UserName,
                    Date = m.CreationDate,
                    Text = m.Text
                }).ToListAsync();

            var chatHistory = new ChatHistory()
            {
                UserName = user.User.UserName,
                UserId = user.Id,
                GroupId = group.Id,
                GroupName = group.Name,
                ChatMessages = chatMessages
            };

            return chatHistory;
        }

        //TODO: Rewrite in more 'friendly' form
        private async Task CloseClientConnectionAsync(string errorMessage)
        {
            //await Clients.Caller.SendAsync("Disconnect", new ChatServerResponse()
            //{
            //    IsSuccessful = false,
            //    Message = errorMessage
            //});
            Context.Abort();
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"New connection with id: [{Context.ConnectionId}]");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogCritical($"Connection id: [{Context.ConnectionId}] is disconnected");
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class GroupInfo
    {
        public string UserName { get; set; } = string.Empty;
        public Guid UserId { get; set; } 
        public Guid GroupId { get; set; } 
        public string GroupName { get; set; } = string.Empty;
    }
}