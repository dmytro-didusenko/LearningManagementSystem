namespace LearningManagementSystem.Domain.Entities
{
    public class Test : BaseEntity
    {
        public Subject Subject { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfStart { get; set; }
        public DateTime DateOfExpiration { get; set; }
        public int DurationInMinutes { get; set; }
        public ICollection<Question> Questions { get; set; } = null!;
        public ICollection<StudentAnswer>? StudentAnswers { get; set; } = null!;
    }
}
