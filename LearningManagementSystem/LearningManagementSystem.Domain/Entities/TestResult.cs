namespace LearningManagementSystem.Domain.Entities
{
    public class TestResult : BaseEntity
    {
        public Guid TestId { get; set; }
        public Guid StudentId { get; set; }
        public DateTime PassingDate { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalAnswers { get; set; }
        public int CorrectAnswers { get; set; }
        public Test Test { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
