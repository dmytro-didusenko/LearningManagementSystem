using System.Drawing;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReportService> _logger;

        public ReportService(AppDbContext context, ILogger<ReportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<StudentReportModel>> GetReportForStudentAsync(Guid studentId)
        {
            var student = await _context.Students
                .Include(i => i.User)
                .Include(g => g.Group)
                .ThenInclude(c => c.Course)
                .ThenInclude(t => t.Subjects)
                .FirstOrDefaultAsync(f => f.Id.Equals(studentId));

            if (student is null)
            {
                return Response<StudentReportModel>.GetError(ErrorCode.NotFound, $"Student with id: {studentId} not found");
            }

            if (student.Group is null || student.Group.Course is null)
            {
                return Response<StudentReportModel>.GetError(ErrorCode.BadRequest, "Student is not yet assigned to any group/course");
            }

            var report = new StudentReportModel
            {
                UserName = student.User.UserName,
                FullName = student.User.FirstName + " " + student.User.LastName,
                GroupName = student.Group.Name,
                CourseName = student.Group.Course.Name
            };

            report.Subjects = _context.TaskAnswers
                .Include(i => i.Grade)
                .Include(i => i.HomeTask)
                .Where(w => w.StudentId.Equals(studentId))
                .Join(_context.Topics.Include(i => i.Subject),
                    f => f.HomeTask.TopicId, s => s.Id,
                    (f, s) => new { taskAnsw = f, topic = s })
                .ToList()
                .GroupBy(g => g.topic.Subject.Name)
                .ToDictionary(k => k.Key, v => v.Select(s => new TopicInfoModel()
                {
                    TopicName = s.topic.Name,
                    Grade = s.taskAnsw.Grade?.Value
                }).AsEnumerable());

            report.ReportCreatedTime = DateTime.Now;

            return Response<StudentReportModel>.GetSuccess(report);
        }

        public async Task GetReportForStudentInExcel(Guid studentId)
        {
            var response = await GetReportForStudentAsync(studentId);
            if (response.Error is not null)
            {
                //TODO: Return error
            }

            //TODO: Check if file is opened

            var report = response.Data!;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fileName = $"{report.FullName}_{report.ReportCreatedTime.ToShortDateString()}.xlsx";
            var fileInfo = new FileInfo($@"C:\Users\Maxim\Desktop\Excels\{fileName}");

            if (fileInfo.Exists)
            {
                fileInfo.Delete();

            }
            using var package = new ExcelPackage(fileInfo);

            var ws = package.Workbook.Worksheets.Add($"{report.FullName}_SuccessReport");
            ws.DefaultColWidth = 25;

            //Header
            var headerCells = ws.Cells[1, 1, 1, 6];
            ws.Cells[1, 1].Value = $"Success report for {report.FullName}, Group: {report.GroupName}, Course: {report.CourseName}";
            headerCells.Merge = true;
            headerCells.AutoFitColumns();
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            ws.Row(1).Height = 50;
            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Font.Size = 18;
            ws.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            headerCells.Style.Fill.SetBackground(Color.Wheat);


            var currCol = 1;
            var currRow = 2;
            foreach (var subject in report.Subjects)
            {
                ws.Cells[currRow, currCol].Value = subject.Key;
                ws.Cells[currRow, currCol, currRow, ++currCol].Merge = true;
                currRow++;
                currCol = 1;
                foreach (var topic in subject.Value)
                {
                    ws.Cells[currRow, currCol].Value = topic.TopicName;
                    ws.Cells[currRow++, currCol + 1].Value = topic.Grade is null ? "Not marked": topic.Grade.Value;
                }

                currRow = 2;
                currCol += 2;
            }


            await package.SaveAsync();
        }


    }
}
