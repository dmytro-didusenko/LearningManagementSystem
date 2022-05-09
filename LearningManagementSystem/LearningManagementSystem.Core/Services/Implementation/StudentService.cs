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
        public async Task CreateStudentAsync(StudentCreationModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            var userExist = await _userService.GetByIdAsync(model.UserId);
            if (userExist is null)
            {
                throw new Exception($"User with id:{model.UserId} does not exist!");
            }

            var student = new Student()
            {
                Id = model.UserId,
                ContractNumber = model.ContractNumber,

            };
            await _context.AddAsync(student);
            await _context.SaveChangesAsync();
        }


        public IEnumerable<StudentModel> GetAll()
        {
            var res = _context.Students.Include(i=>i.User).AsEnumerable();
            return _mapper.Map<IEnumerable<StudentModel>>(res);
        }
    }
}
