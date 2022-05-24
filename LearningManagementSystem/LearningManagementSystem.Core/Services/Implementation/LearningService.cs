using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class LearningService : ILearningService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LearningService> _logger;

        public LearningService(AppDbContext context, IMapper mapper, ILogger<LearningService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<TopicCreateModel>> CreateTopicAsync(TopicCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var subjectExist = await _context.Subjects.FirstOrDefaultAsync(f => f.Id.Equals(model.SubjectId));
            if (subjectExist == null)
            {
                return new Response<TopicCreateModel>()
                {
                    Error = "Subject does not exist",
                    IsSuccessful = false
                };
            }
            var topic = _mapper.Map<Topic>(model);
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            return new Response<TopicCreateModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public async Task<Response<HomeTaskCreateModel>> CreateHomeTaskAsync(HomeTaskCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var topicExist = await _context.Topics.Include(i => i.HomeTask).AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(model.TopicId));
            if (topicExist is null)
            {
                return new Response<HomeTaskCreateModel>()
                {
                    IsSuccessful = false,
                    Error = "Topic does not exist"
                };
            }

            if (topicExist.HomeTask is not null)
            {
                return new Response<HomeTaskCreateModel>()
                {
                    IsSuccessful = false,
                    Error = "Topic already has a Home task"
                };
            }

            var entity = _mapper.Map<HomeTask>(model);
            entity.TopicId = model.TopicId;
            await _context.HomeTasks.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new Response<HomeTaskCreateModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public async Task<Response<HomeTaskModel>> UpdateHomeTaskAsync(Guid id, HomeTaskModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var topic = await _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(id));
            if (topic is null)
            {
                return new Response<HomeTaskModel>()
                {
                    IsSuccessful = false,
                    Error = $"Home task with id:{id} does not exist"
                };
            }
            model.TopicId = id;
            _context.HomeTasks.Update(_mapper.Map<HomeTask>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("Home task[id]:{0} has been updated", model.TopicId);
            return new Response<HomeTaskModel>()
            {
                IsSuccessful = true,
                Data = model
            };
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
            return _mapper.Map<HomeTaskModel>(homeTask);
        }

        public async Task<Response<HomeTaskModel>> AddTaskAnswerAsync(TaskAnswerModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var student = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(model.StudentId));
            if (student is null)
            {
                return new Response<HomeTaskModel>()
                {
                    IsSuccessful = false,
                    Error = "Student does not exist"
                };
            }

            var homeTask = await _context.HomeTasks.FirstOrDefaultAsync(f => f.TopicId.Equals(model.HomeTaskId));
            if (homeTask is null)
            {
                return new Response<HomeTaskModel>()
                {
                    IsSuccessful = false,
                    Error = "Home task does not exist"
                };
            }

            await _context.TaskAnswers.AddAsync(_mapper.Map<TaskAnswer>(model));
            await _context.SaveChangesAsync();

            return new Response<HomeTaskModel>()
            {
                IsSuccessful = true
            };
        }

        public IEnumerable<TaskAnswerModel>? GetTaskAnswersByHomeTaskId(Guid homeTaskId)
        {
            var answers = _context.TaskAnswers.Where(i => i.HomeTaskId.Equals(homeTaskId)).AsEnumerable();
            return _mapper.Map<IEnumerable<TaskAnswerModel>>(answers);
        }

        public async Task<Response<TaskAnswerModel>> UpdateTaskAnswerAsync(Guid id, TaskAnswerUpdateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var taskAnswer = await _context.TaskAnswers
                .Include(i=>i.HomeTask)
                .FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (taskAnswer is null)
            {
                return new Response<TaskAnswerModel>()
                {
                    IsSuccessful = false,
                    Error = "Task does not exist"
                };
            }

            if (taskAnswer.HomeTask.DateOfExpiration <= DateTime.Now)
            {
                return new Response<TaskAnswerModel>()
                {
                    IsSuccessful = false,
                    Error = "Time is out!"
                };
            }

            taskAnswer.Answer = model.Answer;
            taskAnswer.LastUpdated = DateTime.Now;
            _context.TaskAnswers.Update(taskAnswer);
            await _context.SaveChangesAsync();
            return new Response<TaskAnswerModel>()
            {
                IsSuccessful = true,
                Data = _mapper.Map<TaskAnswerModel>(taskAnswer)
            };
        }

        public IEnumerable<TopicModel> GetTopicsBySubjectId(Guid subjectId)
        {
            var topics = _context.Topics.Include(i => i.HomeTask)
                .Where(i => i.SubjectId.Equals(subjectId)).AsEnumerable();
            return _mapper.Map<IEnumerable<TopicModel>>(topics);
        }

        public async Task<Response<TopicModel>> UpdateTopicAsync(Guid id, TopicModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var topic = await _context.Topics.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (topic is null)
            {
                return new Response<TopicModel>()
                {
                    IsSuccessful = false,
                    Error = $"Topic with id:{id} does not exist"
                };
            }
            model.Id = id;
            _context.Topics.Update(_mapper.Map<Topic>(model));
            await _context.SaveChangesAsync();
            _logger.LogInformation("Topic[id]:{0} has been updated", model.Id);
            return new Response<TopicModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }
    }
}
