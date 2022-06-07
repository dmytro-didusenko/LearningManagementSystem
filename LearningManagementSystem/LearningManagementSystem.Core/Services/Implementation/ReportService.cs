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
                .Include(i => i.HomeTask)
                .ThenInclude(t => t.Topic)
                .ThenInclude(t => t.Subject)?
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
            ws.Cells.AutoFitColumns();

            var headerRow = 1;
            ws.Cells[headerRow++, 1].Value = "Success report";
            ws.Cells[headerRow, 1].Value = "Student:";
            ws.Cells[headerRow++, 2].Value = report.FullName;
            ws.Cells[headerRow, 1].Value = "Group:";
            ws.Cells[headerRow++, 2].Value = report.GroupName;
            ws.Cells[headerRow, 1].Value = "Course:";
            ws.Cells[headerRow, 2].Value = report.CourseName;

            var topicsCount = report.Subjects.Values.Max(s => s.Count());

            var headerCells = ws.Cells[1, 1, headerRow, topicsCount + 1];
            headerCells.Style.Font.Size = 15;
            headerCells.Style.Font.Bold = true;
            headerCells.AutoFitColumns();
            headerCells.Style.Fill.SetBackground(Color.DodgerBlue);

            var successRange = ws.Cells[1, 1, 1, topicsCount + 1];
            successRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            var topicRow = headerRow + 1;
            var topicCol = 1;

            for (int j = 0, i = 2; j < topicsCount; j++)
            {
                var topicCells = ws.Cells[topicRow, i];
                topicCells.Value = $"Topic {j + 1}";
                topicCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                topicCells.Style.Fill.SetBackground(Color.Yellow);
                topicCells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                i++;
            }

            var subjRow = topicRow + 1;
            var table = ws.Cells[topicRow, 1, report.Subjects.Count + topicRow, topicsCount + 1];
            table.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            foreach (var subject in report.Subjects)
            {
                var subjCol = 1;
                var subjectCell = ws.Cells[subjRow, subjCol];
                subjectCell.Value = subject.Key;
                subjectCell.AutoFitColumns();
                subjectCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                subjCol++;
                foreach (var topic in subject.Value)
                {
                    var gradeCell = ws.Cells[subjRow, subjCol];
                    gradeCell.Value = topic.Grade.Value;
                    gradeCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    gradeCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    subjCol++;
                }
                subjRow++;
            }
            return Response<(string fileName, byte[] data)>.GetSuccess((reportName, await package.GetAsByteArrayAsync()));
        }

        public GroupReportModel GetReportForGroup(Guid groupId)
        {
            var groupInfo = _context.Groups
                .Include(i => i.Course)
                .FirstOrDefault(f => f.Id.Equals(groupId));

            var report = new GroupReportModel()
            {
                GroupName = groupInfo.Name,
                CourseName = groupInfo.Course.Name
            };

            report.Subjects = _context.TaskAnswers
                .Include(i => i.Grade)
                .Include(i => i.Student)
                .ThenInclude(t => t.User)
                .Include(i => i.HomeTask)
                .ThenInclude(th => th.Topic)
                .ThenInclude(th => th.Subject)
                .Where(w => w.Student.GroupId.Equals(groupId))
                .ToList()
                .GroupBy(g => g.HomeTask.Topic.Subject.Name)
                .ToDictionary(k => k.Key, v =>
                    v.GroupBy(g => g.HomeTask.Topic.Name)
                        .ToDictionary(k => k.Key, v =>
                              v.ToDictionary(k => k.Student.User.UserName, v => v.Grade?.Value)));


            return report;
        }
    }
}
