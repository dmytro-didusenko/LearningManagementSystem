namespace LearningManagementSystem.Domain.Entities
{
    public class Topic : BaseEntity
    {
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public HomeTask? HomeTask { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
