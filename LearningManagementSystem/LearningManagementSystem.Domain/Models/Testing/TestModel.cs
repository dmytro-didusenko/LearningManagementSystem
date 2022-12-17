namespace LearningManagementSystem.Domain.Models.Testing
{
    public class TestModel
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfStart { get; set; }
        public DateTime DateOfExpiration { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
