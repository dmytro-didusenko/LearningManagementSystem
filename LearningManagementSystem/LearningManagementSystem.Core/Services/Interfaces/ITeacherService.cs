using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ITeacherService
    {
        public Task<Response<TeacherCreationModel>> AddAsync(TeacherCreationModel model);
        public Task<TeacherModel> GetByIdAsync(Guid id);
        public IEnumerable<TeacherModel> GetAll();
    }
}
