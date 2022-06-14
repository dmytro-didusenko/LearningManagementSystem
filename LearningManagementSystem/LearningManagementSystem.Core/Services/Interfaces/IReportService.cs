using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IReportService
    {
        public Task<Response<StudentReportModel>> GetReportForStudentAsync(Guid studentId);
        public Task<Response<(string fileName, byte[] data)>> GetReportForStudentInExcel(Guid studentId);
        public Task<Response<GroupReportModel>> GetReportForGroup(Guid groupId);
        public Task<Response<(string fileName, byte[] data)>> GetReportForGroupInExcel(Guid groupId);
        public Task<Response<VisitingReport>> GetVisitingFromExcel(IFormFile visitingReport);
    }
}
