namespace LearningManagementSystem.Domain.Models.HomeTask
{
    public class TaskAnswerUpdateModel
    {
        public Guid Id { get; set; }
        public string Answer { get; set; } = null!;
    }
}
