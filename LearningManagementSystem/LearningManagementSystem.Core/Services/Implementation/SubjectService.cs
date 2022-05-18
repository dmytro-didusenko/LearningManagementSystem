using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(AppDbContext context, IMapper mapper, ILogger<SubjectService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<SubjectModel>> AddAsync(SubjectModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var entity = _mapper.Map<Subject>(model);

            if (model.CoursesIds is not null)
            {
                entity.Courses = new HashSet<Course>();
                foreach (var courseId in model.CoursesIds)
                {
                    var course = await _context.Courses.Include(i => i.Subjects)
                        .SingleOrDefaultAsync(s => s.Id.Equals(courseId));
                    entity.Courses.Add(course);
                }
            }

            await _context.Subjects.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New subject has been successfully added");
            return new Response<SubjectModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public async Task UpdateAsync(Guid id, SubjectModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var entity = await _context.Subjects.AsNoTracking().SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (entity is null)
            {
                throw new Exception($"Subject with id:{id} does not exist");
            }

            model.Id = id;
            var toUpdate = _mapper.Map<Subject>(model);
            _context.Subjects.Update(toUpdate);
            await _context.SaveChangesAsync();
        }

        //TODO: Implement this
        public Task RemoveAsync(SubjectModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<SubjectModel> GetByIdAsync(Guid id)
        {
            var subject = await _context.Subjects
                .Include(i=>i.Courses)
                .Include(i=>i.Teachers).SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (subject is null)
            {
                throw new Exception("Subject does not exist");
            }

            return _mapper.Map<SubjectModel>(subject);
        }

        public IEnumerable<SubjectModel> GetAll()
        {
            var subjects = _context.Subjects.Include(i => i.Courses)
                .Include(i=>i.Teachers).AsEnumerable();
            return _mapper.Map<IEnumerable<SubjectModel>>(subjects);
        }
    }
}
