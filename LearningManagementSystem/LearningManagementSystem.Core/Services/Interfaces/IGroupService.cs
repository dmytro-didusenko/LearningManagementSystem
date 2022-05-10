using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<Response<GroupCreationModel>> AddAsync(GroupCreationModel model);
        public Task UpdateAsync(Guid id, GroupCreationModel model);
        public Task RemoveAsync(GroupModel model);
        public Task<GroupModel> GetByIdAsync(Guid id);
        public IEnumerable<GroupModel> GetAll();
    }
}
