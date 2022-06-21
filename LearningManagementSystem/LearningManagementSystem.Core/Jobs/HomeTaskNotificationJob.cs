using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.MassTransitModels;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Jobs
{
    public class HomeTaskNotificationJob : IJob
    {
        private readonly ILogger<HomeTaskNotificationJob> _logger;
        private readonly IPublishEndpoint _publisher;
        private readonly AppDbContext _context;

        public HomeTaskNotificationJob(AppDbContext context, ILogger<HomeTaskNotificationJob> logger,
            IConfiguration configuration, IPublishEndpoint publisher)
        {
            _logger = logger;
            _publisher = publisher;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tomorrowDate = DateTime.Today.AddDays(1).Date;

            var allTasks = _context.HomeTasks.Where(x => x.DateOfExpiration.Date.Equals(tomorrowDate));

            var completedTaskAnswers = _context.TaskAnswers.Where(x => allTasks.Select(y => y.TopicId).Contains(x.HomeTaskId));

            var students = _context.Students.Where(x => !completedTaskAnswers.Select(y => y.StudentId).Contains(x.Id));

            var users = _context.Users.Where(x => students.Select(y => y.Id).Contains(x.Id));

            var activeUsers = users.Where(x => x.IsActive.Equals(true));



            if (activeUsers is not null)
            {
                foreach (var user in activeUsers)
                {
                    await _publisher.Publish(new ApiMessage()
                    {
                        DeliveryMethod = DeliveryMethod.Email,
                        MessageType = MessageType.Information,
                        Text = $"Dear {user.LastName} {user.FirstName}, today is the last day of homework submission",
                        Receivers = new List<string>() { user.Email }
                    });
                    _logger.LogInformation("Message has been successfully sent!");
                }
            }
        }
    }
}
     
