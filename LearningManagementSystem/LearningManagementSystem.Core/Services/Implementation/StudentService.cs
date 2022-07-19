using AutoMapper;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Responses;
using LearningManagementSystem.Domain.Models.User;
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
        public async Task<Response<StudentCreateModel>> AddAsync(StudentCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var user = await _userService.GetByIdAsync(model.UserId);
            if (user is null)
            {
                return Response<StudentCreateModel>.GetError(ErrorCode.BadRequest, "User with id:{model.UserId} does not exist!");
            }

            var teacher = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(model.UserId));
            var student = await _context.Teachers.FirstOrDefaultAsync(f => f.Id.Equals(model.UserId));

            if (teacher is not null || student is not null)
            {
                return Response<StudentCreateModel>.GetError(ErrorCode.BadRequest, "User already has a role");
            }

            var studentEntity = new Student()
            {
                Id = model.UserId,
                ContractNumber = model.ContractNumber,
            };
            await _context.Students.AddAsync(studentEntity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Adding new student");
            return Response<StudentCreateModel>.GetSuccess(model);
        }

        public async Task<StudentModel> GetByIdAsync(Guid id)
        {
            var entity = await _context.Students.Include(i => i.User).SingleOrDefaultAsync(u => u.Id.Equals(id));
            if (entity is null)
            {
                throw new NotFoundException(id);
            }

            var model = _mapper.Map<StudentModel>(entity);
            _logger.LogInformation("Get student by id:{0}", model.Id);
            return model;
        }

        public async Task<IEnumerable<StudentModel>> GetAll()
        {
            _logger.LogInformation("Getting all students");
            var res = await _context.Students.Include(s => s.User).ToListAsync();
            return _mapper.Map<IEnumerable<StudentModel>>(res);
        }

        public async Task<IEnumerable<StudentModel>> GetStudentsWithoutGroups()
        {
            _logger.LogInformation("Getting students without groups");
            var res = await _context.Students.Include(s => s.User).Where(s => s.GroupId == null).ToListAsync();
            if (res is null)
                throw new NotFoundException("There are no students without groups");
            return _mapper.Map<IEnumerable<StudentModel>>(res);
        }

        public async Task RemoveStudentAsync(Guid id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (student is null)
            {
                throw new NotFoundException(id);
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}