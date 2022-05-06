using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class ManagementService : IManagementService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<ManagementService> _logger;

        public ManagementService(AppDbContext context, IUserService userService, IMapper mapper, ILogger<ManagementService> logger)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GroupModel>> CreateGroupAsync(GroupModel group)
        {
            ArgumentNullException.ThrowIfNull(group);

            var groupExist = await 
                _context.Groups.FirstOrDefaultAsync(f => f.Id.Equals(group.Id) || f.Name.Equals(group.Name));

            if (groupExist is not null)
            {
                _logger.LogInformation("Trying to add group that already exist!");
                return new Response<GroupModel>()
                {
                    IsSuccessful = false,
                    Error = "Group is already exist!"
                };
            }

            //TODO: implement student attachment to group

            await _context.Groups.AddAsync(_mapper.Map<Group>(group));
            await _context.SaveChangesAsync();
            _logger.LogInformation("New group has been created successfully");
            return new Response<GroupModel>()
            {
                IsSuccessful = true, 
                Data = group
            };
        }

        public async Task AddStudentToGroupAsync(Guid groupId, Guid userId)
        {
           var user = await _userService.GetByIdAsync(userId);
           if (user is null)
           {
               throw new Exception("User does not exist!");
           }
           var group = await GetGroupByIdAsync(groupId);
           if (group is null)
           {
               throw new Exception("User does not exist!");
           }

           var student = new Student()
           {
               Id = userId,
               GroupId = groupId
           };
           await _context.AddAsync(student);
           await _context.SaveChangesAsync();
        }

        public async Task<GroupModel> GetGroupByIdAsync(Guid groupId)
        {
            var group = await _context.Groups.Include(i=>i.Students).SingleOrDefaultAsync(f => f.Id.Equals(groupId));
            if (group is null)
            {
                throw new Exception("Group does not exist");
            }
            return _mapper.Map<GroupModel>(group);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            var entities =  _context.Groups.Include(i=>i.Students)
                .ThenInclude(t=>t.User).AsEnumerable();
            return _mapper.Map<IEnumerable<GroupModel>>(entities);
        }
    }
}
