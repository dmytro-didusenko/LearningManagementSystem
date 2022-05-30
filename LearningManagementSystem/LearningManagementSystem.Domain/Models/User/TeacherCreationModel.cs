namespace LearningManagementSystem.Domain.Models
{
    public class TeacherCreationModel
    {
        public Guid UserId { get; set; }
        public Guid? SubjectId { get; set; }
        public string Position { get; set; } = string.Empty;
    }
}
