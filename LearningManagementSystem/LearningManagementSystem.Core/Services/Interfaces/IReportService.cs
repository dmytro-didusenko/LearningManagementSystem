using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IReportService
    {
        public Task<Response<StudentReportModel>> GetReportForStudentAsync(Guid studentId);
        public Task<Response<(string fileName, byte[] data)>> GetReportForStudentInExcel(Guid studentId);
        public Task<Response<GroupReportModel>> GetReportForGroup(Guid groupId);
 
    }
}
