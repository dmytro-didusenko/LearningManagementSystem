namespace LearningManagementSystem.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public Guid? SubjectId { get; set; }
        public User User { get; set; } = null!;
        //TODO: Change to many to many
        public Subject? Subject { get; set; } = null!;
        public string Position { get; set; } = string.Empty;
    }
}
