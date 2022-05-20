namespace LearningManagementSystem.Domain.Models
{
    public class HomeTaskDto
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DatePlannedStart { get; set; }
        public DateTime DateOfExpiration { get; set; }
    }
}
