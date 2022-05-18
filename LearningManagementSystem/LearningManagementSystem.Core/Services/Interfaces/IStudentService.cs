using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IStudentService
    {
        public Task AddAsync(StudentCreationModel model);
        public Task<StudentModel> GetByIdAsync(Guid id);
        public IEnumerable<StudentModel> GetAll();
    }
}
