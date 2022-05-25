using LearningManagementSystem.Domain.Entities;

namespace LearningManagementSystem.Domain.Models
{
    public class DocumentQueryModel
    {
        public Guid? UserId { get; set; }
        public DocumentType? DocumentType { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateOfIssue { get; set; }
        public DateTime? DateOfExpiration{ get; set; }
    }
}
