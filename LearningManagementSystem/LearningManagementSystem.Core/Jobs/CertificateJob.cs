using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.MassTransitModels;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LearningManagementSystem.Core.Jobs
{
    public class CertificateJob : IJob
    {
        private readonly ILogger<CertificateJob> _logger;
        private readonly IPublishEndpoint _publisher;
        private readonly AppDbContext _context;

        public CertificateJob(AppDbContext context, ILogger<CertificateJob> logger, IPublishEndpoint publisher)
        {
            _logger = logger;
            _publisher = publisher;
            _context = context;
        }

        //create certificate for students whose course ended previous day and passed at least 60% of tasks
        public async Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            List<Student> students = _context.Groups
                .Include(g => g.Course)
                .Include(g => g.Students)
                    .ThenInclude(s => s.Group)
                .Include(g => g.Students)
                    .ThenInclude(s => s.User)
                .Where(g => g.EndEducation < today && g.EndEducation >= yesterday)
                .SelectMany(g => g.Students)
                .ToList();

            if (students.Any())
            {
                foreach (var student in students)
                {
                    var studentTasksAnswers = _context.TaskAnswers.Count(ta => ta.StudentId.Equals(student.Id));
                    var totalTasks = _context.Topics
                        .Include(t => t.HomeTask)
                        .Include(t => t.Subject)
                            .ThenInclude(s => s.Courses)
                        .Where(t => t.Subject.Courses
                            .FirstOrDefault(c => c.Id.Equals(student.Group.CourseId)) != null)
                        .Count(t => t.HomeTask != null);

                    if ((double)studentTasksAnswers / (double)totalTasks >= 0.6)
                    {
                        _logger.LogCritical($"\n\nstudent answers:{studentTasksAnswers}, total:{totalTasks}");
                        Certificate certificate = new Certificate()
                        {
                            StudentId = student.Id,
                            CourseId = (Guid)student.Group.CourseId,
                            Date = DateTime.Now,
                            Student = student,
                            Course = student.Group.Course
                        };
                        await _context.Certificates.AddAsync(certificate);
                        await _context.SaveChangesAsync();
                    }

                var receivers = students.Select(s => s.User.Email);
                var message = new ApiMessage()
                {
                    MessageType = MessageType.Information,
                    DeliveryMethod = DeliveryMethod.Email,
                    Receivers = receivers,
                    Subject = "Congratulations on your certification!",
                    Text = "We are happy to announce that you have successfully got certificated."
                };

                await _publisher.Publish(message);
                _logger.LogInformation("Certification message was successfully sent.");
                }
            }
        }
    }
}