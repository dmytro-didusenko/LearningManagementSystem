namespace LearningManagementSystem.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Teacher>? Teachers { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Topic>? Topics { get; set; }
        public Test? Test { get; set; }
        public Guid? TestId { get; set; }
    }
}
