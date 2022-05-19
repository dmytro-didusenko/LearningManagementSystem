using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IDocumentService
    {
        public Task<Response<DocumentModel>> AddDocumentAsync(DocumentModel document);
        public Task<IEnumerable<DocumentModel>> GetDocumentsByFilterAsync(DocumentQueryModel query);
        public Task RemoveDocumentByIdAsync(Guid id);
    }
}
