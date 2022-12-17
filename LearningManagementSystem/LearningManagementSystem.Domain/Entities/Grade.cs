namespace LearningManagementSystem.Domain.Entities
{
    public class Grade
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; } = string.Empty;
        public TaskAnswer TaskAnswer { get; set; } = null!;
    }
}
