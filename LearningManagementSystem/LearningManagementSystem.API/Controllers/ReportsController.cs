using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("Progress/{studentId}")]
        public async Task<IActionResult> GetReportForStudent(Guid studentId)
        {
            var res = await _reportService.GetReportForStudentAsync(studentId);
            return res.ToActionResult();
        }

        [HttpGet("Progress/{studentId}/Excel")]
        public async Task<IActionResult> GetReportForStudentExcel(Guid studentId)
        {
            var res = await _reportService.GetReportForStudentInExcel(studentId);
            if (res.Error is not null)
            {
                return BadRequest(res.Error.ErrorMessage);
            }
            return File(res.Data.data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", res.Data.fileName);
        }

        [HttpGet("Group/{groupId}")]
        public async Task<IActionResult> GetGroupReport(Guid groupId)
        {
            var res = await _reportService.GetReportForGroup(groupId);
            return  res.ToActionResult();
        }

        [HttpGet("Progress/Group/{groupId}/Excel")]
        public async Task<IActionResult> GetReportForGroupExcel(Guid groupId)
        {
            var res = await _reportService.GetReportForGroupInExcel(groupId);
            if (res.Error is not null)
            {
                return BadRequest(res.Error.ErrorMessage);
            }
            return File(res.Data.data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", res.Data.fileName);
        }

        [HttpPost("Visiting")]
        public async Task<IActionResult> GetVisitingFromExcel(IFormFile visitingReport)
        {
            var res = await _reportService.GetVisitingFromExcel(visitingReport);
            return res.ToActionResult();
        }

    }
}
