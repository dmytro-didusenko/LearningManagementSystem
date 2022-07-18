using AutoMapper;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Group;
using LearningManagementSystem.Domain.Models.Responses;
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

        public async Task<Response<GroupModel>> AddAsync(GroupCreateModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var groupExist = await
                _context.Groups.FirstOrDefaultAsync(f => f.Id.Equals(model.Id) || f.Name.Equals(model.Name));

            if (groupExist is not null)
            {
                _logger.LogInformation("Trying to add group that already exist!");
                return Response<GroupModel>.GetError(ErrorCode.Conflict, "Group with such name exists.");
            }
            var group = _mapper.Map<Group>(model);
            await _context.Groups.AddAsync(group);

            var res = _mapper.Map<GroupModel>(group);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New group has been created successfully");
            return Response<GroupModel>.GetSuccess(res);
        }

        public async Task UpdateAsync(Guid id, GroupCreateModel model)
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

        public async Task RemoveAsync(Guid id)
        {
            var group = await _context.Groups.AsNoTracking().FirstOrDefaultAsync(f => f.Id.Equals(id));
            if (group is null)
            {
                throw new Exception($"Group id:{id} does not exist!");
            }
            group.IsActive = !group.IsActive;
            _context.Groups.Update(group);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Group[id]:{0} has been deleted", group.Id);
        }

        public async Task<GroupModel> GetByIdAsync(Guid id)
        {
            var group = await _context.Groups.Include(i=>i.Students).ThenInclude(t=>t.User).SingleOrDefaultAsync(s => s.Id.Equals(id) && s.IsActive);
            if (group is null)
            {
                throw new NotFoundException(id);
            }
            return _mapper.Map<GroupModel>(group);
        }

        public IEnumerable<GroupModel> GetAll()
        {
            return _mapper.Map<IEnumerable<GroupModel>>(_context.Groups
                .Include(i=>i.Students).ThenInclude(t=>t.User)
                //.Where(w=>w.IsActive)
                .ToList());
        }
    }
}