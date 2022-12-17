using System.Runtime.Serialization;

namespace LearningManagementSystem.Domain.Models.Topic
{
    public class TopicCreateModel
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateOfCreation { get; set; }
    }
}
