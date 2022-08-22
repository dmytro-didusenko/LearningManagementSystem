using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.Testing;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ITestingService
    {
        public Task<Response<TestModel>> CreateTestAsync(TestModel model);
        public Task<Response<QuestionCreateModel>> AddQuestionAsync(Guid testId, QuestionCreateModel questionModel);
        public Task<Response<IEnumerable<AnswerCreateModel>>> AddAnswersToQuestionAsync(Guid questionId,
            IEnumerable<AnswerCreateModel> answers);
        public Task<TestModel?> GetTestByIdAsync(Guid testId);
        public IEnumerable<QuestionCreateModel>? GetQuestionsByTestId(Guid testId);
        public Task<QuestionPassingModel> GetQuestionsForPassing(Guid testId);
        public Task<Response<TestResultModel>> PassTest(IEnumerable<StudentAnswerModel> models);
        public Task<TestResultModel> GetTestingResultAsync(Guid testId, Guid studentId);
    }
}
