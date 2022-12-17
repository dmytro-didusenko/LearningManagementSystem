using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.Subject;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ISubjectService
    {
        public Task<Response<SubjectModel>> AddAsync(SubjectModel model);
        public Task<Response<SubjectModel>> UpdateAsync(Guid id, SubjectModel model);
        public Task<SubjectModel> GetByIdAsync(Guid id);
        Task<IEnumerable<SubjectModel>> GetAll();
    }
}
