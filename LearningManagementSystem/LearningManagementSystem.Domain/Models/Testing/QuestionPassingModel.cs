namespace LearningManagementSystem.Domain.Models.Testing
{
    public class QuestionPassingModel
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IEnumerable<AnswerPassingModel> Answers { get; set; } = null!;
    }
}
