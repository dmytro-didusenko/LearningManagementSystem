using LearningManagementSystem.Domain.Models.Course;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ICourseService : IBaseService<CourseModel>
    {
        public IEnumerable<CourseModel> GetAll();
    }
}
