namespace LearningManagementSystem.Domain.Models
{
    public class TaskAnswerUpdateModel
    {
        public Guid Id { get; set; }
        public string Answer { get; set; } = null!;
    }
}
