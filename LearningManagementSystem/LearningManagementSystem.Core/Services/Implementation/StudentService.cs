using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(AppDbContext context, IUserService userService, IMapper mapper, ILogger<StudentService> logger)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task AddAsync(StudentCreationModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var userExist = await _userService.GetByIdAsync(model.UserId);
            if (userExist is null)
            {
                throw new Exception($"User with id:{model.UserId} does not exist!");
            }

            var teacher = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(model.UserId));
            var student = await _context.Teachers.FirstOrDefaultAsync(f => f.Id.Equals(model.UserId));

            if (teacher is not null || student is not null)
            {
                throw new Exception($"User with id:{model.UserId} does not exist!");
            }

            var studentEntity = new Student()
            {
                Id = model.UserId,
                ContractNumber = model.ContractNumber,
            };
            await _context.Students.AddAsync(studentEntity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Adding new student");
        }

        public async Task<StudentModel> GetByIdAsync(Guid id)
        {
            var entity = await _context.Students.Include(i => i.User).SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (entity is null)
            {
                throw new Exception("Student does not exist!");
            }

            var model = _mapper.Map<StudentModel>(entity);
            _logger.LogInformation("Get student by id:{0}", model.Id);
            return model;
        }

        public IEnumerable<StudentModel> GetAll()
        {
            _logger.LogInformation("Getting all students");
            var res = _context.Students.Include(i => i.User).AsEnumerable();
            return _mapper.Map<IEnumerable<StudentModel>>(res);
        }

        public async Task RemoveStudentAsync(Guid id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (student is null)
            {
                throw new Exception("Student does not exist");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}