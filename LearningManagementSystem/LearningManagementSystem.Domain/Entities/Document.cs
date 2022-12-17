namespace LearningManagementSystem.Domain.Entities
{
    public class Document: BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DocumentType DocumentType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime? DateOfExpiration{ get; set; }
    }

    public enum DocumentType
    {
        TeacherLicense,
        StudentCertificate,
        Certificate, 
        License
    }
}
