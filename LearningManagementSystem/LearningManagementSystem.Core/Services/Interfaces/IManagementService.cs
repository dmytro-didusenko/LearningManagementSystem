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
        public Task<Response<GroupModel>> CreateGroupAsync(GroupModel group);
        public Task AddStudentToGroup(Guid groupId, StudentModel student);
        public IEnumerable<GroupModel> GetAll();
    }
}
