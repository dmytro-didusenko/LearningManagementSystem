using LearningManagementSystem.Domain.Models.Group;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<Response<GroupCreateModel>> AddAsync(GroupCreateModel model);
        public Task UpdateAsync(Guid id, GroupCreateModel model);
        public Task RemoveAsync(Guid id);
        public Task<GroupModel> GetByIdAsync(Guid id);
        public IEnumerable<GroupModel> GetAll();
    }
}
