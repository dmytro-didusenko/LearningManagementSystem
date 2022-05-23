namespace LearningManagementSystem.Domain.Models
{
    public class HomeTaskCreateModel
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DatePlannedStart { get; set; }
        public DateTime DateOfExpiration { get; set; }
    }
}
