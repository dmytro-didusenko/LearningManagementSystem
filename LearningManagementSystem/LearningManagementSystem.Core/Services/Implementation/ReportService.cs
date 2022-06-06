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

        public async Task<Response<(string fileName, byte[] data)>> GetReportForStudentInExcel(Guid studentId)
        {
            var response = await GetReportForStudentAsync(studentId);
            if (response.Error is not null)
            {
                return Response<(string fileName, byte[] data)>.GetError(response.Error.ErrorCode, response.Error.ErrorMessage);
            }

            var report = response.Data!;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var reportName = $"{report.FullName}_{report.ReportCreatedTime.ToShortDateString()}.xlsx";

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add($"{report.FullName}_SuccessReport");
            ws.DefaultColWidth = 25;

            ////Header
            var headerText = ws.Cells[1, 1];
            headerText.Style.VerticalAlignment = ExcelVerticalAlignment.Top;

            headerText.Value = "Success reports" + "\r\n" +
                               $"Student: {report.FullName}\r\n" +
                               $"Group: {report.GroupName}\r\n" +
                               $"Course: {report.CourseName}\r\n";
            headerText.Style.WrapText = true;
            headerText.Style.Font.Size = 16;
            headerText.Style.Font.Bold = true;

            var minHeaderWidth = report.Subjects.Count * 2 >= 6 ? report.Subjects.Count * 2 : 6;
            var headerCells = ws.Cells[1, 1, 1, minHeaderWidth];
            headerCells.Merge = true;
            headerCells.Style.Fill.SetBackground(Color.Wheat);
            ws.Row(1).Height = 82;

            var subjectRow = 2;
            var subjectCol = 1;

            for (int j=0, i = 2; j <= report.Subjects.Values.Count; j++)
            {
                ws.Cells[subjectRow, i++].Value = $"Topic {j + 1}";
            }

            var subjRow = subjectRow+1;
        
            foreach (var subject in report.Subjects)
            {
                var subjCol = 1;
                ws.Cells[subjRow, subjCol].Value = subject.Key;
                subjCol++;
                foreach (var topic in subject.Value)
                {
                    ws.Cells[subjRow, subjCol++].Value = topic.Grade.Value;
                }
                subjRow++;
            }
            return Response<(string fileName, byte[] data)>.GetSuccess((reportName, await package.GetAsByteArrayAsync()));
        }
    }
}
