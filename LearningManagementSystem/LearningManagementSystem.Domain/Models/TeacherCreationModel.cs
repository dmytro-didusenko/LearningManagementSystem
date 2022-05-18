namespace LearningManagementSystem.Domain.Models
{
    public class TeacherCreationModel
    {
        public Guid Id { get; set; }
        public Guid? SubjectId { get; set; }
        public string Position { get; set; } = string.Empty;
    }
}
