namespace LearningManagementSystem.Domain.Models.Certificate
{
    public class CertificateModel
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get { return "Certificate"; } }
        public DateTime Date { get; set; }
        public string StudentName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
    }
}