using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}
