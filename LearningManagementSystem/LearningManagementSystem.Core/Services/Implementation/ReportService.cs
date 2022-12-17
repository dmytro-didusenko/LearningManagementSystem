﻿using System.Data;
using AutoMapper;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Models.Options;
using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReportService> _logger;
        private readonly IMapper _mapper;
        private readonly VisitingReportModel vrModel;

        public ReportService(AppDbContext context, ILogger<ReportService> logger,
            IMapper mapper, IOptions<VisitingReportOptions> visitingReportOptions)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            vrModel = _mapper.Map<VisitingReportModel>(visitingReportOptions.Value);
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
                return Response<StudentReportModel>.GetError(ErrorCode.NotFound,
                    $"Student with id: {studentId} not found");
            }

            if (student.Group is null || student.Group.Course is null)
            {
                return Response<StudentReportModel>.GetError(ErrorCode.BadRequest,
                    "Student is not yet assigned to any group/course");
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
                return Response<(string fileName, byte[] data)>.GetError(response.Error.ErrorCode,
                    response.Error.ErrorMessage);
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

            var topicsCount = report.Subjects!.Values.Max(s => s.Count());

            var headerCells = ws.Cells[1, 1, headerRow, topicsCount + 1];
            ExcelStyleHelper.AddStyles(headerCells, new List<SuccessReportStyles>() { SuccessReportStyles.HeaderStyling });

            var successRange = ws.Cells[1, 1, 1, topicsCount + 1];
            successRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

            var topicRow = headerRow + 1;

            for (int j = 0, i = 2; j < topicsCount; j++)
            {
                var topicCells = ws.Cells[topicRow, i];
                topicCells.Value = $"Topic {j + 1}";
                ExcelStyleHelper.AddStyles(topicCells, new List<SuccessReportStyles>() { SuccessReportStyles.TopicStyling });
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
                    gradeCell.Value = topic.Grade!.Value;
                    ExcelStyleHelper.AddStyles(gradeCell, new List<SuccessReportStyles>() { SuccessReportStyles.CenteringAndBorderThinStyling });
                    subjCol++;
                }
                subjRow++;
            }

            return Response<(string fileName, byte[] data)>.GetSuccess(
                (reportName, await package.GetAsByteArrayAsync()));
        }

        public async Task<Response<GroupReportModel>> GetReportForGroup(Guid groupId)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            string query = $"SELECT [g].[Name] AS 'Group', [c].[Name] AS 'Course', [sb].[Name] AS 'Subject'," +
                           $"[t].[Name] AS 'Topic', CONCAT([u].[FirstName], ' ', [u].[LastName]) AS 'Student', [gr].[Value] AS 'Grade'" +
                           $"FROM [dbo].[Groups] AS [g]" +
                           $"JOIN [dbo].[Courses] AS [c] ON [c].[Id] = [g].[CourseId]" +
                           $"JOIN [dbo].[CourseSubject] AS [cs] ON [cs].[CoursesId] = [c].[Id]" +
                           $"JOIN [dbo].[Subjects] AS [sb] ON [sb].[Id] = [cs].[SubjectsId]" +
                           $"JOIN [dbo].[Topics] AS [t] ON [t].[SubjectId] = [sb].[Id]" +
                           $"JOIN [dbo].[Students] AS [s] ON [s].[GroupId] = [g].[Id]" +
                           $"JOIN [dbo].[Users] AS [u] ON [u].[Id] = [s].[Id]" +
                           $"JOIN [dbo].[HomeTasks] AS [ht] ON [ht].[TopicId] = [t].[Id]" +
                           $"LEFT JOIN [dbo].[TaskAnswers] AS [ta] ON [ta].[HomeTaskId] = [ht].[TopicId] AND [ta].[StudentId] = [s].[Id]" +
                           $"LEFT JOIN [dbo].[Grades] AS [gr] ON [gr].[Id] = [ta].[Id]" +
                           $"WHERE [g].[Id] = '{groupId}'";

            command.CommandText = query;
            command.CommandType = CommandType.Text;

            await _context.Database.OpenConnectionAsync();

            using var result = await command.ExecuteReaderAsync();
            if (!result.HasRows)
            {
                return Response<GroupReportModel>.GetError(ErrorCode.BadRequest, "There are no data!");
            }
            var queryResult = new List<ReportQueryModel>();
            
            while (await result.ReadAsync())
            {
                queryResult.Add(new ReportQueryModel()
                {
                    Group = result.GetString("Group"),
                    Course = result.GetString("Course"),
                    Subject = result.GetString("Subject"),
                    Topic = result.GetString("Topic"),
                    Student = result.GetString("Student"),
                    Grade = result.GetValue("Grade").ToString()
                });
            }
            var report = new GroupReportModel()
            {
                GroupName = queryResult.FirstOrDefault().Group,
                CourseName = queryResult.FirstOrDefault().Course
            };

            report.Subjects = queryResult.GroupBy(g => g.Subject)
                .ToDictionary(k => k.Key, v => v.GroupBy(g => g.Topic)
                    .ToDictionary(k => k.Key,
                        v => v.ToDictionary(k => k.Student, v => v.Grade)));

            report.ReportCreatedTime = DateTime.Now;
            return Response<GroupReportModel>.GetSuccess(report);
        }

        public async Task<Response<(string fileName, byte[] data)>> GetReportForGroupInExcel(Guid groupId)
        {
            var response = await GetReportForGroup(groupId);
            if (response.Error is not null)
            {
                return Response<(string fileName, byte[] data)>.GetError(response.Error.ErrorCode,
                    response.Error.ErrorMessage);
            }

            var report = response.Data!;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var reportName = $"{report.GroupName.Replace(" ", "_")}_{report.ReportCreatedTime.ToShortDateString()}.xlsx";

            using var package = new ExcelPackage();

            var subjects = report.Subjects;

            foreach (var subject in subjects)
            {
                var ws = package.Workbook.Worksheets.Add($"{subject.Key.Replace(" ", "_")}");
                ws.Cells.AutoFitColumns();

                //Header
                var headerRow = 1;
                ws.Cells[headerRow++, 1].Value = "Success report";
                ws.Cells[headerRow, 1].Value = "Group:";
                var collToFit = ws.Cells[headerRow++, 2];
                collToFit.Value = report.GroupName;
                ws.Cells[headerRow, 1].Value = "Course:";
                ws.Cells[headerRow, 2].Value = report.CourseName;

                var topicsCount = subject.Value.Count;

                var headerCells = ws.Cells[1, 1, headerRow, topicsCount + 1];
                ExcelStyleHelper.AddStyles(headerCells, new List<SuccessReportStyles>() { SuccessReportStyles.HeaderStyling });

                var successRange = ws.Cells[1, 1, 1, topicsCount + 1];
                successRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                var subjectRow = headerRow + 1;
                var subjectCol = 2;

                //Students
                var students = report.Subjects.Values
                    .SelectMany(s => s.Values).SelectMany(s => s.Keys).Distinct().ToList();

                var studRow = subjectRow + 2;
                foreach (var stud in students)
                {
                    var studCell = ws.Cells[studRow, 1];
                    studCell.Value = stud;
                    studCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    studRow++;
                }

                //Date
                var dateCells = ws.Cells[subjectRow, 1, subjectRow + 1, 1];
                dateCells.Value = report.ReportCreatedTime.ToShortDateString();
                dateCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                dateCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                dateCells.Merge = true;
                dateCells.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                //Subject
                var subjectNameCell = ws.Cells[subjectRow, subjectCol];
                subjectNameCell.Value = subject.Key;
                var cellToMerge = subjectCol + subject.Value.Count - 1;
                var subjectCells = ws.Cells[subjectRow, subjectCol, subjectRow, cellToMerge];
                subjectCells.Merge = true;
                ExcelStyleHelper.AddStyles(subjectCells, new List<SuccessReportStyles>() { SuccessReportStyles.SubjectStyling });


                //Topics
                foreach (var topic in subject.Value)
                {
                    var topicRow = subjectRow + 1;
                    var topicCol = subjectCol;
                    var topicCells = ws.Cells[topicRow, subjectCol];
                    topicCells.Value = topic.Key;
                    ExcelStyleHelper.AddStyles(topicCells, new List<SuccessReportStyles>() { SuccessReportStyles.TopicStyling });

                    var gradeRow = topicRow + 1;
                    foreach (var grade in topic.Value)
                    {
                        var gradeCol = topicCol;
                        var gradeCell = ws.Cells[gradeRow++, gradeCol++];
                        gradeCell.Value = grade.Value == string.Empty ? "X" : grade.Value;
                        ExcelStyleHelper.AddStyles(gradeCell, new List<SuccessReportStyles>() { SuccessReportStyles.CenteringAndBorderThinStyling });
                    }

                    subjectCol++;
                }
                collToFit.AutoFitColumns();
            }

            return Response<(string fileName, byte[] data)>.GetSuccess(
                (reportName, await package.GetAsByteArrayAsync()));
        }

        public async Task<Response<VisitingReport>> GetVisitingFromExcel(IFormFile visitingReport)
        {
            if (!visitingReport.FileName.EndsWith(".xlsx"))
            {
                return Response<VisitingReport>.GetError(ErrorCode.BadRequest, "File should be .xlsx type");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            await package.LoadAsync(visitingReport.OpenReadStream());

            var result = new VisitingReport();

            var wsFirst = package.Workbook.Worksheets.FirstOrDefault();
            if (wsFirst is null)
            {
                return Response<VisitingReport>.GetError(ErrorCode.BadRequest, "Report does not contains any WorkSheets");
            }

            result.GroupName = wsFirst.Cells[vrModel.GroupCell.row, vrModel.GroupCell.col].Value?.ToString()!;
            result.CourseName = wsFirst.Cells[vrModel.CourseCell.row, vrModel.CourseCell.col].Value?.ToString()!;
            result.ReportCreatedTime = DateTime.Parse(wsFirst.Cells[vrModel.DateCell.row, vrModel.DateCell.col].Value?.ToString()!);

            foreach (var ws in package.Workbook.Worksheets)
            {
                var subjectName = ws.Cells[vrModel.SubjectCell.row, vrModel.SubjectCell.col].Value?.ToString();

                var endCol = ws.Dimension.Columns;
                var endRow = ws.Dimension.Rows;
                var topicsDict = new Dictionary<string, Dictionary<string, string>>();
                for (int i = vrModel.TopicsStartCell.col; i <= endCol; i++)
                {
                    var topic = ws.Cells[vrModel.TopicsStartCell.row, i].Value?.ToString();

                    var gradeCol = i;
                    var studRow = vrModel.StudentsStartCell.row;
                    var gradesDict = new Dictionary<string, string>();
                    for (int j = vrModel.StudentsStartCell.row; j <= endRow; j++)
                    {
                        var student = ws.Cells[j, vrModel.StudentsStartCell.col].Value?.ToString();
                        var grade = ws.Cells[j, gradeCol].Value?.ToString();
                        gradesDict.Add(student!, grade!);
                    }
                    topicsDict.Add(topic!, gradesDict);
                }
                result.Subjects.Add(subjectName!, topicsDict);
            }

            return Response<VisitingReport>.GetSuccess(result);
        }
    }
}