using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IStudentService
    {
        public Task AddAsync(StudentCreationModel model);
        public Task<StudentModel> GetByIdAsync(Guid id);
        public IEnumerable<StudentModel> GetAll();
    }
}
