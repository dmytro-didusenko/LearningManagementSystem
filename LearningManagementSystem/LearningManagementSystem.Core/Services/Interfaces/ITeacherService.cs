using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.User;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ITeacherService
    {
        public Task<Response<TeacherCreateModel>> AddAsync(TeacherCreateModel model);
        public Task<TeacherModel> GetByIdAsync(Guid id);
        public IEnumerable<TeacherModel> GetAll();
        public Task RemoveTeacherAsync(Guid id);
    }
}
