using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IReportService
    {
        public Task<Response<StudentReportModel>> GetReportForStudent(Guid studentId);
    }
}
