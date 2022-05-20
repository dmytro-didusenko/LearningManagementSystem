using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IHomeTaskService
    {
        public Task<Response<HomeTaskDto>> CreateHomeTaskAsync(HomeTaskDto model);
        public IEnumerable<HomeTaskModel> GetHomeTasksBySubjectId(Guid subjectId);

        //TODO: Remove this
        public IEnumerable<HomeTaskModel> GetAllHomeTasks();

    }
}
