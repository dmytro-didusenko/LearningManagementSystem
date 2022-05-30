namespace LearningManagementSystem.Domain.Models
{
    public class TeacherModel : UserModel
    {
        public Guid? SubjectId { get; set; }
        public string Position { get; set; } = string.Empty;
    }
}
