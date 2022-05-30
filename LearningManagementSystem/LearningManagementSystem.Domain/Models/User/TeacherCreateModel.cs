namespace LearningManagementSystem.Domain.Models.User
{
    public class TeacherCreateModel
    {
        public Guid UserId { get; set; }
        public Guid? SubjectId { get; set; }
        public string Position { get; set; } = string.Empty;
    }
}
