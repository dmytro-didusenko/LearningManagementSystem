namespace LearningManagementSystem.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}
