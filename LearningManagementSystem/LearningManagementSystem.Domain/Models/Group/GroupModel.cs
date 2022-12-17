using LearningManagementSystem.Domain.Models.User;

namespace LearningManagementSystem.Domain.Models.Group
{
    public class GroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartEducation { get; set; }
        public DateTime EndEducation { get; set; }
        public Guid? CourseId { get; set; }
        public ICollection<StudentModel>? Students { get; set; } = null;
        public bool IsActive { get; set; }
    }
}
