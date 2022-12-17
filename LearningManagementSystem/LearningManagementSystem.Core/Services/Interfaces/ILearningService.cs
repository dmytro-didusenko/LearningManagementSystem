﻿using LearningManagementSystem.Domain.Models.HomeTask;
using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.Topic;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ILearningService
    {
        public Task<Response<TopicCreateModel>> CreateTopicAsync(TopicCreateModel model);
        public Task<IEnumerable<TopicModel>> GetTopicsBySubjectId(Guid subjectId);
        public Task<Response<TopicModel>> UpdateTopicAsync(Guid id, TopicModel model);
        public Task<Response<HomeTaskCreateModel>> CreateHomeTaskAsync(HomeTaskCreateModel model);
        public Task<Response<HomeTaskModel>> UpdateHomeTaskAsync(Guid id, HomeTaskModel model);
        public Task<Response> RemoveHomeTaskAsync(Guid topicId);
        public Task<HomeTaskModel?> GetHomeTaskByIdAsync(Guid topicId);
        public Task<Response<TaskAnswerModel>> AddTaskAnswerAsync(TaskAnswerModel model);
        public Task<IEnumerable<GradeModel>> GetStudentGrades(Guid studentId);
        public IEnumerable<TaskAnswerModel>? GetTaskAnswersByHomeTaskId(Guid homeTaskId);
        public Task<Response<TaskAnswerModel>> UpdateTaskAnswerAsync(Guid id, TaskAnswerUpdateModel model);
        public Task<Response<GradeModel>> AddGradeAsync(Guid taskAnswerId, GradeModel model);
    }
}
