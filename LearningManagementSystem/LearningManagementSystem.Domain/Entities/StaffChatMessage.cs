namespace LearningManagementSystem.Domain.Entities
{
    public class StaffChatMessage : BaseEntity
    {
        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string Text { get; set; } = null!;
    }
}
