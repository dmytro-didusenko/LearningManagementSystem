using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.MassTransitModels;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LearningManagementSystem.Core.Jobs
{
    public class CourseStartingJob : IJob
    {
        private readonly ILogger<CourseStartingJob> _logger;
        private readonly IPublishEndpoint _publisher;
        private readonly AppDbContext _context;

        public CourseStartingJob(AppDbContext context, ILogger<CourseStartingJob> logger,
            IConfiguration configuration, IPublishEndpoint publisher)
        {
            _logger = logger;
            _publisher = publisher;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.Today;
            var actual = _context.Courses.Where(i =>
                i.StartedAt.Day.Equals(today.Day) &&
                (i.StartedAt.Month - 1).Equals(today.Month)).AsEnumerable();

            var usersToSend = await _context.Students.Include(i => i.User)
                .Select(s => s.User.FirstName + " " + s.User.LastName)
                .ToListAsync();

            if (actual is not null && actual.Any())
            {
                foreach (var course in actual)
                {
                    await _publisher.Publish(new ApiMessage()
                    {
                        DeliveryMethod = DeliveryMethod.Email,
                        MessageType = MessageType.Information,
                        Text = $"New course {course.Name} starts next month",
                        Receivers = usersToSend
                    });
                    _logger.LogInformation("Message has been successfully sent!");
                }
            }
        }
    }
}
