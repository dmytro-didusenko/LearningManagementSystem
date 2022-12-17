namespace LearningManagementSystem.Domain.ChatModels
{
    public class ChatUserModel
    {
        public string UserName { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
    }
}
