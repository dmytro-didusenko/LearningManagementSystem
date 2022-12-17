using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.ChatModels;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class ChatService : IChatService
    {
        private readonly AppDbContext _db;

        public ChatService(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddMessageAsync(ChatUserModel user, ChatMessage message)
        {
            await _db.GroupChatMessages.AddAsync(new GroupChatMessage()
            {
                SenderId = user.UserId,
                GroupId = user.GroupId,
                Text = message.Text,
                CreationDate = message.Date
            });
            await _db.SaveChangesAsync();
        }

        public async Task<ChatUserModel?> GetChatUser(Guid userId)
        {
            var user = await _db.Students
                .Include(i => i.Group)
                .Include(i => i.User)
                .FirstOrDefaultAsync(f => f.Id.Equals(userId));
            
            if (user is null || user.Group is null)
            {
                throw new HubException("User does not exist!");
            }

            var userModel = new ChatUserModel()
            {
                GroupId = user!.Group!.Id,
                GroupName = user.Group.Name,
                UserId = user.Id,
                UserName = user.User.UserName
            };
            return userModel;
        }

        public async Task<IEnumerable<ChatMessage>> GetChatHistory(ChatUserModel user)
        {
            var chatMessages = await _db.GroupChatMessages
                .Where(w => w.GroupId.Equals(user.GroupId))
                .Select(m => new ChatMessage()
                {
                    Sender = m.Sender.UserName,
                    Date = m.CreationDate,
                    Text = m.Text
                }).ToListAsync();

            return chatMessages;
        }
    }
}
