using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GroupService> _logger;

        public GroupService(AppDbContext context, IMapper mapper, ILogger<GroupService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<GroupCreationModel>> AddAsync(GroupCreationModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var groupExist = await
                _context.Groups.FirstOrDefaultAsync(f => f.Id.Equals(model.Id) || f.Name.Equals(model.Name));

            if (groupExist is not null)
            {
                _logger.LogInformation("Trying to add group that already exist!");
                return new Response<GroupCreationModel>()
                {
                    IsSuccessful = false,
                    Error = "Group is already exist!"
                };
            }
            await _context.Groups.AddAsync(_mapper.Map<Group>(model));

            await _context.SaveChangesAsync();
            _logger.LogInformation("New group has been created successfully");
            return new Response<GroupCreationModel>()
            {
                IsSuccessful = true,
                Data = model
            };
        }

        public async Task UpdateAsync(Guid id, GroupCreationModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var group = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(id));

            if (group is null)
            {
                throw new Exception($"Group id:{id} does not exist!");
            }

            model.Id = id;
            _context.Groups.Update(_mapper.Map<Group>(model));

            await _context.SaveChangesAsync();
            _logger.LogInformation("Group[id]:{0} has been updated", group.Id);
        }

        //TODO: Implement this
        public Task RemoveAsync(GroupModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<GroupModel> GetByIdAsync(Guid id)
        {
            var group = await _context.Groups.Include(i=>i.Students).ThenInclude(t=>t.User).SingleOrDefaultAsync(s => s.Id.Equals(id));
            if (group is null)
            {
                throw new Exception($"Group with id: {id} does not exist");
            }
            return _mapper.Map<GroupModel>(group);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            return _mapper.Map<IEnumerable<GroupModel>>(_context.Groups
                .Include(i=>i.Students).ThenInclude(t=>t.User).AsEnumerable());
        }
    }
}