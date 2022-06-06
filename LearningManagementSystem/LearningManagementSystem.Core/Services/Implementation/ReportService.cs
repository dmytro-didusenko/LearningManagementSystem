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
                .Include(i => i.Grade)?
                .Include(i => i.HomeTask)?
                .ThenInclude(t => t.Topic)?
                .ThenInclude(t => t.Subject)
                .Where(w => w.StudentId.Equals(studentId))
                .ToList()
                .GroupBy(g => g.HomeTask.Topic.Subject.Name)
                .ToDictionary(k => k.Key, v => v.Select(s => new TopicInfoModel()
                {
                    TopicName = s.HomeTask.Topic.Name,
                    Grade = s.Grade?.Value
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
            var minHeaderWidth = report.Subjects.Count * 2 >= 6 ? report.Subjects.Count * 2 : 6;
            var headerCells = ws.Cells[1, 1, 1, minHeaderWidth];
            ws.Cells[1, 1].Value = $"Success report for {report.FullName}, Group: {report.GroupName}, Course: {report.CourseName}";
            headerCells.Merge = true;
            headerCells.AutoFitColumns();
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
            ws.Row(1).Height = 50;
            ws.Row(1).Style.Font.Bold = true;
            ws.Row(1).Style.Font.Size = 18;
            ws.Row(1).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            headerCells.Style.Fill.SetBackground(Color.Wheat);

            var subjectRow = 2;
            var subjectCol = 1;
            foreach (var subject in report.Subjects)
            {
                ws.Cells[subjectRow, subjectCol].Value = subject.Key;
                var subjectNameCells = ws.Cells[subjectRow, subjectCol, subjectRow, ++subjectCol];

                subjectNameCells.Merge = true;
                subjectNameCells.Style.Font.Bold = true;
                subjectNameCells.Style.Font.Size = 15;
                subjectNameCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                subjectNameCells.Style.Fill.SetBackground(Color.Green);
                subjectNameCells.Style.Border.BorderAround(ExcelBorderStyle.Hair);

                var topicRow = subjectRow + 1;
                foreach (var topic in subject.Value)
                {
                    var topicCol = subjectCol - 1;
                    var topicCells = ws.Cells[topicRow, topicCol];
                    topicCells.Value = topic.TopicName;
                    topicCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    topicCells.Style.Fill.SetBackground(Color.Yellow);
                    topicCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    var gradeCells = ws.Cells[topicRow, ++topicCol];
                    gradeCells.Value = topic.Grade is null ? "Not marked" : topic.Grade.Value;
                    gradeCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    topicRow++;
                }
                subjectCol++;
            }
            
            await package.SaveAsync();
        }
    }
}
