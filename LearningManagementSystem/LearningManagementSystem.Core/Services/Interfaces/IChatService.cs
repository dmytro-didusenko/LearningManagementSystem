using LearningManagementSystem.Domain.ChatModels;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IChatService
    {
        public Task AddMessageAsync(ChatUserModel user, ChatMessage message);
        public Task<ChatUserModel?> GetChatUser(Guid userId);
        public Task<IEnumerable<ChatMessage>> GetChatHistory(ChatUserModel user);
    }
}
