namespace LearningManagementSystem.Domain.Models.Subject
{
    public class SubjectModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Guid>? CoursesIds { get; set; }
        public IEnumerable<Guid>? TeachersIds { get; set; }
        public IEnumerable<Guid>? TopicsIds { get; set; }
    }
}
