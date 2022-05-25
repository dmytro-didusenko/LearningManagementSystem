
namespace LearningManagementSystem.Domain.Entities
{
    public class Question : BaseEntity
    {
        public Guid TestId { get; set; }
        public Test Test { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Answer>? Answers { get; set; }
    }
}
