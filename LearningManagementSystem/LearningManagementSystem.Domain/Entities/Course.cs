namespace LearningManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
