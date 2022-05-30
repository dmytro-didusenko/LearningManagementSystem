using LearningManagementSystem.Domain.Models.HomeTask;

namespace LearningManagementSystem.Domain.Models.Topic
{
    public class TopicModel
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateOfCreation { get; set; }
        public HomeTaskModel? HomeTaskModel { get; set; }
    }
}
