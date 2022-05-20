
namespace LearningManagementSystem.Domain.Entities
{
    public class HomeTask : BaseEntity
    {
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DatePlannedStart { get; set; }
        public DateTime DateOfExpiration { get; set; }
        public ICollection<TaskAnswer>? TaskAnswers { get; set; }
    }
}
