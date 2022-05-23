using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ITopicService
    {
        public Task<Response<TopicCreateModel>> CreateTopic(TopicCreateModel model);
        public Task<Response<HomeTaskCreateModel>> CreateHomeTask(HomeTaskCreateModel model);
        public IEnumerable<TopicModel> GetAllTopics();

    }
}
