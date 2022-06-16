using LearningManagementSystem.Domain.ChatModels;

namespace LearningManagementSystem.API.Hubs.ClientsInterfaces
{
    public interface IChatClient
    {
        public Task Send(ChatMessage message);
    }
}
