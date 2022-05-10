namespace LearningManagementSystem.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = null!;
        public DateTime StartEducation { get; set; }
        public DateTime EndEducation { get; set; }
        public ICollection<Student> Students { get; set; } = null!;
    }
}
