
namespace LearningManagementSystem.Domain.Models.Testing
{
    public class TestResultModel
    {
        public Guid TestId { get; set; }
        public Guid StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalAnswers { get; set; }
        public int CorrectAnswers { get; set; }
    }
}
