using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class HomeTaskService : IHomeTaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeTaskService> _logger;

        public HomeTaskService(AppDbContext context, IMapper mapper, ILogger<HomeTaskService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<HomeTaskDto>> CreateHomeTaskAsync(HomeTaskDto model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var subjectExist = await _context.Subjects.FirstOrDefaultAsync(f => f.Id.Equals(model.SubjectId));
            if (subjectExist is null)
            {
                return new Response<HomeTaskDto>()
                {
                    IsSuccessful = false,
                    Error = "Subject does not exist"
                };
            }

            await _context.HomeTasks.AddAsync(_mapper.Map<HomeTask>(model));
            await _context.SaveChangesAsync();

            return new Response<HomeTaskDto>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public IEnumerable<HomeTaskModel> GetHomeTasksBySubjectId(Guid subjectId)
        {
            var tasks = _context.HomeTasks.Where(i => i.SubjectId.Equals(subjectId));
            return _mapper.Map<IEnumerable<HomeTaskModel>>(tasks);
        }

        public IEnumerable<HomeTaskModel> GetAllHomeTasks()
        {
            var tasks = _context.HomeTasks.Include(i=>i.TaskAnswers)
                .AsEnumerable();
            return _mapper.Map<IEnumerable<HomeTaskModel>>(tasks);
        }
    }
}
