using AutoMapper;
using Hangfire;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.HangfireJobs;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.HomeTask;
using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.Topic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class LearningService : ILearningService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LearningService> _logger;
        private readonly IBackgroundJobClient _jobClient;

        public LearningService(AppDbContext context, 
            IMapper mapper, 
            ILogger<LearningService> logger,
            IBackgroundJobClient jobClient)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _jobClient = jobClient;
        }

        public async Task<Response<TopicCreateModel>> CreateTopicAsync(TopicCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var subjectExist = await _context.Subjects.FirstOrDefaultAsync(f => f.Id.Equals(model.SubjectId));
            if (subjectExist == null)
            {
                return Response<TopicCreateModel>.GetError(ErrorCode.NotFound, "Subject does not exist");
            }
            var topic = _mapper.Map<Topic>(model);
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();

            return Response<TopicCreateModel>.GetSuccess(model);
        }

        public async Task<Response<HomeTaskCreateModel>> CreateHomeTaskAsync(HomeTaskCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var topicExist = await _context.Topics.Include(i => i.HomeTask)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id.Equals(model.TopicId));
            if (topicExist is null)
            {
                return Response<HomeTaskCreateModel>.GetError(ErrorCode.NotFound, "Topic does not exist");
            }

            if (topicExist.HomeTask is not null)
            {
                return Response<HomeTaskCreateModel>.GetError(ErrorCode.Conflict, "Topic already has a Home task");
            }

            var entity = _mapper.Map<HomeTask>(model);
            entity.TopicId = model.TopicId;
            await _context.HomeTasks.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Response<HomeTaskCreateModel>.GetSuccess(model);
        }

        public async Task<Response<HomeTaskModel>> UpdateHomeTaskAsync(Guid id, HomeTaskModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var topic = await _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(id));
            if (topic is null)
            {
                return Response<HomeTaskModel>.GetError(ErrorCode.NotFound, $"Home task with id:{id} does not exist");
            }
            model.TopicId = id;
            _context.HomeTasks.Update(_mapper.Map<HomeTask>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("Home task[id]:{0} has been updated", model.TopicId);

            return Response<HomeTaskModel>.GetSuccess(model);
        }

        public async Task<Response> RemoveHomeTaskAsync(Guid topicId)
        {
            var homeTask = await _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(topicId));
            if (homeTask is null)
            {
                return new Response()
                {
                    IsSuccessful = false,
                    Error = $"Home task with id:{topicId} does not exist"
                };
            }

            _context.HomeTasks.Remove(homeTask);
            await _context.SaveChangesAsync();
            return new Response()
            {
                IsSuccessful = true
            };
        }

        public async Task<HomeTaskModel?> GetHomeTaskByIdAsync(Guid topicId)
        {
            var homeTask = await _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(topicId));
            if (homeTask is null)
            {
                throw new NotFoundException(topicId);
            }
            return _mapper.Map<HomeTaskModel>(homeTask);
        }

        public async Task<Response<TaskAnswerModel>> AddTaskAnswerAsync(TaskAnswerModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var student = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(model.StudentId));
            if (student is null)
            {
                return Response<TaskAnswerModel>.GetError(ErrorCode.NotFound, "Student does not exist");
            }

            var homeTask = await _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(model.HomeTaskId));
            if (homeTask is null)
            {
                return Response<TaskAnswerModel>.GetError(ErrorCode.Conflict, "Home task does not exist");
            }

            var entity = _mapper.Map<TaskAnswer>(model);
            await _context.TaskAnswers.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Response<TaskAnswerModel>.GetSuccess(model);

        }

        public async Task<IEnumerable<GradeModel>> GetStudentGrades(Guid studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(studentId));
            if (student is null)
            {
                throw new NotFoundException("Student does not exist");
            }
            var grades = _context.TaskAnswers
                .Include(i => i.Grade)
                .Where(i => i.StudentId.Equals(studentId))
                .Select(s => s.Grade)
                .AsEnumerable();

            return _mapper.Map<IEnumerable<GradeModel>>(grades);
        }

        //TODO: Rewrite logic
        public IEnumerable<GradeModel>? GetStudentGradesBySubjectId(Guid studentId, Guid subjectId)
        {
            var grades = _context.Topics
                .Include(i => i.HomeTask)
                .ThenInclude(t => t.TaskAnswers)
                .ThenInclude(t => t.Grade)
                .Where(i => i.SubjectId.Equals(subjectId))
                .Select(s => s.HomeTask)
                .SelectMany(s => s.TaskAnswers)
                .Where(i => i.StudentId.Equals(studentId))
                .Select(s => s.Grade)
                .ToListAsync().Result;

            var mapped = _mapper.Map<IEnumerable<GradeModel>>(grades);
            return mapped;
        }

        public IEnumerable<TaskAnswerModel>? GetTaskAnswersByHomeTaskId(Guid homeTaskId)
        {
            var homeTask = _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(homeTaskId));
            if (homeTask is null)
            {
                throw new NotFoundException(homeTaskId);
            }
            var answers = _context.TaskAnswers
                .Where(i => i.HomeTaskId.Equals(homeTaskId)).AsEnumerable();
            return _mapper.Map<IEnumerable<TaskAnswerModel>>(answers);
        }

        public async Task<Response<TaskAnswerModel>> UpdateTaskAnswerAsync(Guid id, TaskAnswerUpdateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var taskAnswer = await _context.TaskAnswers
                .Include(i => i.HomeTask)
                .FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (taskAnswer is null)
            {
                return Response<TaskAnswerModel>.GetError(ErrorCode.NotFound, "Task does not exist");
            }

            if (taskAnswer.HomeTask.DateOfExpiration <= DateTime.Now)
            {
                return Response<TaskAnswerModel>.GetError(ErrorCode.Conflict, "Time is out!");
            }

            taskAnswer.Answer = model.Answer;
            taskAnswer.LastUpdated = DateTime.Now;
            _context.TaskAnswers.Update(taskAnswer);
            await _context.SaveChangesAsync();

            return Response<TaskAnswerModel>.GetSuccess(_mapper.Map<TaskAnswerModel>(taskAnswer));
        }

        public async Task<Response<GradeModel>> AddGradeAsync(Guid taskAnswerId, GradeModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var taskAnswer = await _context.TaskAnswers.FirstOrDefaultAsync(f => f.Id.Equals(taskAnswerId));
            if (taskAnswer is null)
            {
                return Response<GradeModel>.GetError(ErrorCode.NotFound, "Task answer does not exist");
            }

            //Running background task
            _jobClient.Enqueue<IGradeNotifyJob>(gradeJob => gradeJob.SendNotification(taskAnswer.StudentId));

            var entity = _mapper.Map<Grade>(model);
            entity.Id = taskAnswerId;
            await _context.Grades.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Response<GradeModel>.GetSuccess(model);
        }

        public async Task<IEnumerable<TopicModel>> GetTopicsBySubjectId(Guid subjectId)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(f => f.Id.Equals(subjectId));
            if (subject is null)
            {
                throw new NotFoundException(subjectId);
            }
            var topics = await _context.Topics
                .Where(i => i.SubjectId.Equals(subjectId)).ToListAsync();
            return _mapper.Map<IEnumerable<TopicModel>>(topics);
        }

        public async Task<Response<TopicModel>> UpdateTopicAsync(Guid id, TopicModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var topic = await _context.Topics.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (topic is null)
            {
                return Response<TopicModel>.GetError(ErrorCode.NotFound, $"Topic with id:{id} does not exist");
            }
            model.Id = id;
            _context.Topics.Update(_mapper.Map<Topic>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("Topic[id]:{0} has been updated", model.Id);

            return Response<TopicModel>.GetSuccess(model);
        }
    }
}
