using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IManagementService
    {
        public Task<Response<GroupCreationModel>> CreateGroupAsync(GroupCreationModel group);
        public Task AddStudentToGroupAsync(Guid groupId, Guid userId);
        public IEnumerable<GroupModel> GetAll();
        public Task<GroupModel> GetGroupByIdAsync(Guid groupId);
        public Task MoveStudentToOtherGroupAsync(Guid studentId, Guid groupId);
        public Task UpdateGroupAsync(Guid groupId, GroupModel group);
    }
}
