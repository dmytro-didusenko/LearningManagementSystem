using LearningManagementSystem.Domain.Models.Subject;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ISubjectService : IBaseService<SubjectModel>
    {
        Task<IEnumerable<SubjectModel>> GetAll();
    }
}
