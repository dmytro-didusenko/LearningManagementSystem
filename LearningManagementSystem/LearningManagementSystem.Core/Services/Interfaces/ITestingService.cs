using LearningManagementSystem.Domain.Models;
using LearningManagementSystem.Domain.Models.Testing;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ITestingService
    {
        public Task<Response<TestModel>> CreateTestAsync(TestModel model);
        public Task<Response<QuestionModel>> AddQuestionAsync(Guid testId, QuestionModel questionModel);
        public Task<Response<IEnumerable<AnswerCreateModel>>> AddAnswersToQuestion(Guid questionId,
            IEnumerable<AnswerCreateModel> answers);
    }
    
}
