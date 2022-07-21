using LearningManagementSystem.Core.Filters;
using LearningManagementSystem.Domain.Models.Group;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<Response<GroupModel>> AddAsync(GroupCreateModel model);
        public Task UpdateAsync(Guid id, GroupCreateModel model);
        public Task RemoveAsync(Guid id);
        public Task<GroupModel> GetByIdAsync(Guid id);
        public Task<PagedResponse<IEnumerable<GroupModel>>> GetAll(PaginationFilter filter);
    }
}
