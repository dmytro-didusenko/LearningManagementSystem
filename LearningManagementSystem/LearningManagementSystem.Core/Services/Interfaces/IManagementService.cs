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
        public Task AddStudentToGroupAsync(Guid studentId, Guid groupId);
        public Task AddCourseToGroup(Guid courseId, Guid groupId);
    }
}
