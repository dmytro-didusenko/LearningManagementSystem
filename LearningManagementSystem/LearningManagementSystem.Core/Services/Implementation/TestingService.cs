using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class TestingService : ITestingService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TestingService> _logger;

        public TestingService(AppDbContext context, IMapper mapper, ILogger<TestingService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<TestModel>> CreateTestAsync(TestModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var subject = await _context.Subjects
                .FirstOrDefaultAsync(f => f.Id.Equals(model.SubjectId));
            if (subject is null)
            {
                return Response<TestModel>.GetError(ErrorCode.NotFound, "Subject does not exist");
            }

            var entity = _mapper.Map<Test>(model);
            if (model.DurationInHours is not null)
            {
                entity.Duration = new TimeSpan(model.DurationInHours!.Value, 0, 0);
            }

            await _context.Tests.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New test has been successfully added!");
            model.Id = entity.Id;
            return Response<TestModel>.GetSuccess(model);
        }

        public async Task<Response<QuestionCreateModel>> AddQuestionAsync(Guid testId, QuestionCreateModel questionModel)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(f => f.Id.Equals(testId));
            if (test is null)
            {
                return Response<QuestionCreateModel>.GetError(ErrorCode.NotFound,"Test does not exist");
            }

            var question = _mapper.Map<Question>(questionModel);
            question.TestId = testId;
            question.Id = Guid.NewGuid();
            if (question.Answers is not null)
            {
                foreach (var answer in question.Answers)
                {
                    answer.QuestionId = question.Id;
                }
            }

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return Response<QuestionCreateModel>.GetSuccess(questionModel);
        }

        public async Task<Response<IEnumerable<AnswerCreateModel>>> AddAnswersToQuestionAsync(Guid questionId,
            IEnumerable<AnswerCreateModel> answers)
        {
            ArgumentNullException.ThrowIfNull(answers);
            var question = await _context.Questions.FirstOrDefaultAsync(f => f.Id.Equals(questionId));
            if (question is null)
            {
                return Response<IEnumerable<AnswerCreateModel>>.GetError(ErrorCode.NotFound,"Question does not exist");
            }

            var answerEntities = _mapper.Map<IEnumerable<Answer>>(answers);
            answerEntities = answerEntities.Select(s =>
            {
                s.QuestionId = questionId;
                return s;
            });

            await _context.AddRangeAsync(answerEntities);
            await _context.SaveChangesAsync();
            return Response<IEnumerable<AnswerCreateModel>>.GetSuccess(answers);
        }

        public async Task<TestModel?> GetTestByIdAsync(Guid testId)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(f => f.Id.Equals(testId));
            return _mapper.Map<TestModel>(test);
        }

        public IEnumerable<QuestionCreateModel>? GetQuestionsByTestId(Guid testId)
        {
            var questions = _context.Questions
                .Include(i => i.Answers)
                .Where(i => i.TestId.Equals(testId)).AsEnumerable();
            return _mapper.Map<IEnumerable<QuestionCreateModel>>(questions);
        }

        public IEnumerable<QuestionPassingModel> GetQuestionsForPassing(Guid testId)
        {
            var questions = _context.Questions
                .Include(i => i.Answers)
                .Where(i => i.TestId.Equals(testId)).AsEnumerable();
            return _mapper.Map<IEnumerable<QuestionPassingModel>>(questions);
        }

        public async Task<Response<IEnumerable<StudentAnswerModel>>> AddStudentAnswersAsync(IEnumerable<StudentAnswerModel> models)
        {
            ArgumentNullException.ThrowIfNull(models);
            var answers = _mapper.Map<IEnumerable<StudentAnswer>>(models);
            await _context.StudentAnswers.AddRangeAsync(answers);
            await _context.SaveChangesAsync();

            return Response<IEnumerable<StudentAnswerModel>>.GetSuccess(models);
        }

        public async Task<TestResultModel> GetTestingResultAsync(Guid testId, Guid studentId)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(f => f.Id.Equals(testId));
            var result = new TestResultModel()
            {
                TestId = testId,
                Name = test.Name,
            };
            var answers = _context.StudentAnswers
                .Include(i=>i.Answer)
                .Where(i => i.StudentId.Equals(studentId) && i.TestId.Equals(testId));
            result.TotalAnswers = answers.Count();
            result.CorrectAnswers = answers.Count(w => w.Answer.IsCorrect);

            return result;
        }

    }
}
