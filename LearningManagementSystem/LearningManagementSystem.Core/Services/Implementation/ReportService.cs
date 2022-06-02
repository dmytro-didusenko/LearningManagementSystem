using System.Text.Json;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

        public async Task<Response<StudentReportModel>> GetReportForStudent(Guid studentId)
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

            report.Subjects = new Dictionary<string, IEnumerable<TopicInfoModel>>();

            var results = _context.Topics?
                .Include(i => i.Subject)?
                .Include(i => i.HomeTask)?
                .ThenInclude(t => t.TaskAnswers)?
                .ThenInclude(t => t.Grade)
                .Select(s => new
                {
                    subject = s.Subject.Name,
                    topicName = s.Name,
                    grade = s.HomeTask.TaskAnswers
                        .FirstOrDefault(w => w.StudentId.Equals(studentId)).Grade
                })
                .ToList()
                .GroupBy(g => g.subject);
                
            //foreach (var q in results)
            //{
            //    _logger.LogCritical("Subject: {0}, topic: {1}, grade: {2}", q.subject, q.topicName, q.grade?.Value);
            //}


            foreach (var res in results)
            {
                report.Subjects.Add(res.Key, res.Select(s => new TopicInfoModel()
                {
                    TopicName = s.topicName,
                    Grade = s.grade?.Value
                }));
            }

            report.ReportCreatedTime = DateTime.Now;

            return Response<StudentReportModel>.GetSuccess(report);
        }
    }
}
