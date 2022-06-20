namespace LearningManagementSystem.Domain.Entities
{
    public class Group : BaseEntity
    { 
        public Guid? CourseId { get; set; }
        public Course? Course { get; set; } = null;
        public string Name { get; set; } = null!;
        public DateTime StartEducation { get; set; }
        public DateTime EndEducation { get; set; }
        public ICollection<Student> Students { get; set; } = null!;
        public ICollection<GroupChatMessage> ChatMessages { get; set; } = null!;
    }
}
