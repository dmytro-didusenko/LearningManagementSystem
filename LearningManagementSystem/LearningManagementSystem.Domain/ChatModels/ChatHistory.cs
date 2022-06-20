namespace LearningManagementSystem.Domain.ChatModels
{
    public class ChatHistory
    {
        public string GroupName { get; set; } = null!;
        public Guid GroupId { get; set; }

        public IEnumerable<ChatMessage> ChatMessages { get; set; } = null!;
    }
}
