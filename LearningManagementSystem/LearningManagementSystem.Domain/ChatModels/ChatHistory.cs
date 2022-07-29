namespace LearningManagementSystem.Domain.ChatModels
{
    public class ChatHistory
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string GroupName { get; set; } = null!;
        public Guid GroupId { get; set; }
        public IEnumerable<ChatMessage> ChatMessages { get; set; } = null!;
    }
}
