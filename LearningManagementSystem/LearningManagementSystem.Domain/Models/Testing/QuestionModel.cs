namespace LearningManagementSystem.Domain.Models.Testing
{
    public class QuestionModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<AnswerCreateModel>? Answers { get; set; }
    }
}
