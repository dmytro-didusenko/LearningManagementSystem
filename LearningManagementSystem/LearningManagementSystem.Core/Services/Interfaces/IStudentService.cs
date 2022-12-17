using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.User;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<Response<StudentCreateModel>> AddAsync(StudentCreateModel model);
        public Task<StudentModel> GetByIdAsync(Guid id);
        Task<IEnumerable<StudentModel>> GetAll();
        Task<IEnumerable<StudentModel>> GetStudentsWithoutGroups();
        public Task RemoveStudentAsync(Guid id);
    }
}
