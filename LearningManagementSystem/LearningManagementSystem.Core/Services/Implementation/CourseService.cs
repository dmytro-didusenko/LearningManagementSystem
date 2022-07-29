using AutoMapper;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Course;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFileHelper _fileHelper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(AppDbContext context,
            IMapper mapper,
            IFileHelper fileHelper,
            ILogger<CourseService> logger)
        {
            _context = context;
            _mapper = mapper;
            _fileHelper = fileHelper;
            _logger = logger;
        }

        public async Task<Response<CourseModel>> AddAsync(CourseModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            model.IsActive = true;
            var entity = _mapper.Map<Course>(model);
            await _context.Courses.AddAsync(entity);
            await _context.SaveChangesAsync();
            model.Id = entity.Id;
            return Response<CourseModel>.GetSuccess(model);
        }

        public async Task<Response<CourseModel>> UpdateAsync(Guid id, CourseModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var courseEntity = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (courseEntity is null)
            {
                return Response<CourseModel>.GetError(ErrorCode.BadRequest, "Course id:{id} does not exist!");
            }

            model.Id = id;
            var entityToUpdate = _mapper.Map<Course>(model);
            _logger.LogCritical($"{entityToUpdate.StartedAt}");
           
            _context.Courses.Update(entityToUpdate);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Course[id]:{0} has been updated", model.Id);
            return Response<CourseModel>.GetSuccess(model);
        }

        public async Task RemoveAsync(Guid id)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(i => i.Id.Equals(
                id));

            if (course is null)
            {
                _logger.LogInformation("Course with id:{0} does not exist", id);
                throw new Exception("Course does not exist");
            }
            course.IsActive = !course.IsActive;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Course {0} has been removed", id);
        }

        public async Task<CourseModel> GetByIdAsync(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (course is null)
            {
                throw new NotFoundException(id);
            }
            return _mapper.Map<CourseModel>(course);
        }

        public IEnumerable<CourseModel> GetAll()
        {
            return _mapper.Map<IEnumerable<CourseModel>>(_context.Courses.ToList());
        }
    }
}