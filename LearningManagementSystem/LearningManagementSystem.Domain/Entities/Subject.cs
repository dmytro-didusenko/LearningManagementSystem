namespace LearningManagementSystem.Domain.Entities
{
    public class Subject : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Course>? Courses { get; set; }
    }
}
