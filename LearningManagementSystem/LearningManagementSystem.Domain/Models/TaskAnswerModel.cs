namespace LearningManagementSystem.Domain.Models
{
    public class TaskAnswerModel
    {
        public Guid Id { get; set; }
        public Guid HomeTaskId { get; set; }
        public Guid StudentId { get; set; }
        public string Answer { get; set; } = null!;
        public DateTime DateOfAnswer { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}
