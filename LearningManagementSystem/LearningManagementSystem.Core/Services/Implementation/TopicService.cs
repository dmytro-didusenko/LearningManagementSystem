using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class TopicService : ITopicService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TopicService> _logger;

        public TopicService(AppDbContext context, IMapper mapper, ILogger<TopicService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<TopicCreateModel>> CreateTopic(TopicCreateModel model)
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

        public async Task<Response<HomeTaskCreateModel>> CreateHomeTask(HomeTaskCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var topicExist = await _context.Topics.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(model.TopicId));
            if (topicExist == null)
            {
                return new Response<HomeTaskCreateModel>()
                {
                    IsSuccessful = false,
                    Error = "Topic does not exist"
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

        //TODO: Remove this
        public IEnumerable<TopicModel> GetAllTopics()
        {
            var topic = _context.Topics.Include(i => i.HomeTask).AsEnumerable();
            return _mapper.Map<IEnumerable<TopicModel>>(topic);
        }

        public IEnumerable<TopicModel> GetTopicsBySubjectId(Guid subjectId)
        {
            var topics = _context.Topics.Include(i=>i.HomeTask)
                .Where(i => i.SubjectId.Equals(subjectId)).AsEnumerable();
            return _mapper.Map<IEnumerable<TopicModel>>(topics);
        }

    }
}
