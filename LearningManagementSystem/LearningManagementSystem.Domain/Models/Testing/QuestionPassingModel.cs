namespace LearningManagementSystem.Domain.Models.Testing
{
    public class QuestionPassingModel
    {
        public int DurationInMinutes { get; set; }
        public ICollection<QuestionCreateModel>? Questions { get; set; }
    }
}
