using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly AppDbContext _db;
        private readonly IChatService _chatService;

        public ChatHub(ILogger<ChatHub> logger,
            AppDbContext db,
            IChatService chatService)
        {
            _logger = logger;
            _db = db;
            _chatService = chatService;
        }

        public async Task MessageHandler(ChatMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                return;
            }
            var user = GetCurrentUser();
            
            await _chatService.AddMessageAsync(user, message);

            await Clients.Group(user.GroupName).SendAsync("ReceiveMessage", message);
        }

        public async Task Handshake(string userId)
        {
            if (!Guid.TryParse(userId, out var parsedId))
            {
                await CloseClientConnectionAsync("Wrong user data");
            }

            var user = await _chatService.GetChatUser(parsedId);

            await Groups.AddToGroupAsync(Context.ConnectionId, user!.GroupName);
            Context.Items.TryAdd("User", user);
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

        public async Task<IEnumerable<ChatMessage>> GetChatHistory()
        {
            var user = GetCurrentUser();

            var chatHistory = await _chatService.GetChatHistory(user);

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