namespace LearningManagementSystem.Domain.Models.Testing
{
    public class StudentAnswerModel
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public Guid StudentId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
