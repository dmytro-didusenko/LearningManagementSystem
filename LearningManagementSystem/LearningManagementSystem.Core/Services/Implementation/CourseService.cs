using AutoMapper;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly FileHelper _fileHelper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(AppDbContext context,
            IMapper mapper,
            FileHelper fileHelper,
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
            var entity = _mapper.Map<Course>(model);
            if (model.Image is not null)
            {
                var path = await _fileHelper.UploadFileAsync(model.Image);
                entity.ImagePath = path;
            }

            await _context.Courses.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new Response<CourseModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public async Task UpdateAsync(Guid id, CourseModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var courseEntity = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (courseEntity is null)
            {
                throw new Exception($"Course id:{id} does not exist!");
            }

            var entityToUpdate = _mapper.Map<Course>(model);
            if (model.Image is not null)
            {
                var path = await _fileHelper.UploadFileAsync(model.Image);
                entityToUpdate.ImagePath = path;
            }

            model.Id = id;
            _context.Courses.Update(entityToUpdate);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Course[id]:{0} has been updated", model.Id);
        }

        public Task RemoveAsync(CourseModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<CourseModel> GetByIdAsync(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (course is null)
            {
                throw new Exception($"Course with id:{id} does not exist");
            }
            return _mapper.Map<CourseModel>(course);
        }

        public IEnumerable<CourseModel> GetAll()
        {
            return _mapper.Map<IEnumerable<CourseModel>>(_context.Courses.AsEnumerable());
        }
    }
}
