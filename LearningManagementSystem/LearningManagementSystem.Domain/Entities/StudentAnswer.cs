namespace LearningManagementSystem.Domain.Entities
{
    public class StudentAnswer : BaseEntity
    {
        public Guid TestId { get; set; }
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
