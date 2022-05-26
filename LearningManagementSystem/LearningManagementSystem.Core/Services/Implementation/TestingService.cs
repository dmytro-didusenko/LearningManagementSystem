using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
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
                return Response<TestModel>.Error("Subject does not exist");
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
            return Response<TestModel>.Success(model);
        }

        public async Task<Response<QuestionModel>> AddQuestionAsync(Guid testId, QuestionModel questionModel)
        {
            var test = await _context.Tests.FirstOrDefaultAsync(f => f.Id.Equals(testId));
            if (test is null)
            {
                return Response<QuestionModel>.Error("Test does not exist");
            }

            var question= _mapper.Map<Question>(questionModel);
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

            return Response<QuestionModel>.Success(questionModel);
        }

        public async Task<Response<IEnumerable<AnswerCreateModel>>> AddAnswersToQuestion(Guid questionId,
            IEnumerable<AnswerCreateModel> answers)
        {
            ArgumentNullException.ThrowIfNull(answers);
            var question = await _context.Questions.FirstOrDefaultAsync(f => f.Id.Equals(questionId));
            if (question is null)
            {
                return Response<IEnumerable<AnswerCreateModel>>.Error("Question does not exist");
            }

            var answerEntities = _mapper.Map<IEnumerable<Answer>>(answers);
            answerEntities = answerEntities.Select(s =>
            {
                s.QuestionId = questionId;
                return s;
            });

            await _context.AddRangeAsync(answerEntities);
            await _context.SaveChangesAsync();
            return Response<IEnumerable<AnswerCreateModel>>.Success(answers);
        }
    }
}
