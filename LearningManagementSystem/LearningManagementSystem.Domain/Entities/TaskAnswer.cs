namespace LearningManagementSystem.Domain.Entities
{
    public class TaskAnswer : BaseEntity
    {
        public Guid HomeTaskId { get; set; }
        public HomeTask HomeTask { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public DateTime DateOfAnswer { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Grade? Grade{ get; set; }
    }
}
