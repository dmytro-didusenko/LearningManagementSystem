using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ICourseService : IBaseService<CourseModel>
    {
        public IEnumerable<CourseModel> GetAll();
    }
}
