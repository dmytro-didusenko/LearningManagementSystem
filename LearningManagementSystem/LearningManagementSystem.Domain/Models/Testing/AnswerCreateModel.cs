namespace LearningManagementSystem.Domain.Models.Testing
{
    public class AnswerCreateModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } = false;
    }
}
