using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IMapper _mapper;
        private readonly ILogger<ManagementService> _logger;

        public ManagementService(AppDbContext context, IMapper mapper, ILogger<ManagementService> logger)
        {
            _context = context;
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

            await _context.Groups.AddAsync(_mapper.Map<Group>(group));
            await _context.SaveChangesAsync();
            _logger.LogInformation("New group has been created successfully");
            return new Response<GroupModel>()
            {
                IsSuccessful = true, 
                Data = group
            };
        }

        public Task AddStudentToGroup(Guid groupId, StudentModel student)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupModel> GetAll()
        {
            var entites =  _context.Groups.AsEnumerable();

            return _mapper.Map<IEnumerable<GroupModel>>(entites);
        }
    }
}
