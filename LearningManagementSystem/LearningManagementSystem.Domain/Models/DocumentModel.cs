using LearningManagementSystem.Domain.Entities;

namespace LearningManagementSystem.Domain.Models
{
    public class DocumentModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime? DateOfExpiration { get; set; }
    }
}
