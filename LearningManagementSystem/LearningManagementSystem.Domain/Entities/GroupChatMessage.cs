namespace LearningManagementSystem.Domain.Entities
{
    public class GroupChatMessage : BaseEntity
    {
        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;
        public Group Group { get; set; } = null!;
        public Guid GroupId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; } = null!;
    }
}
