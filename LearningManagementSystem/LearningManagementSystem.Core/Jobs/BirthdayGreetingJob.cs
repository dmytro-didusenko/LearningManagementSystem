using System.Text;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.MassTransitModels;
using LearningManagementSystem.Domain.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using RabbitMQ.Client;

namespace LearningManagementSystem.Core.Jobs
{
    public class BirthdayGreetingJob : IJob
    {
        private readonly ILogger<BirthdayGreetingJob> _logger;
        private readonly IPublishEndpoint _publisher;
        private readonly AppDbContext _context;

        public BirthdayGreetingJob(AppDbContext context, ILogger<BirthdayGreetingJob> logger,
            IConfiguration configuration, IPublishEndpoint publisher)
        {
            _logger = logger;
            _publisher = publisher;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.Today;
            var toGreat = _context.Users.AsNoTracking().Where(i => i.Birthday.Day.Equals(today.Day)
                                                                   && i.Birthday.Month.Equals(today.Month)).AsEnumerable();
            if (toGreat is not null && toGreat.Any())
            {
                foreach (var user in toGreat)
                {
                    var message = new ApiMessage()
                    {
                        DeliveryMethod = DeliveryMethod.Email,
                        MessageType = MessageType.Greeting,
                        To = user.Email,
                        Subject = "Happy Birthday!",
                        Text = $"Happy birthday, {user.FirstName} {user.LastName}"
                    };
                    await _publisher.Publish(message);
                    _logger.LogInformation("Message has been successfully sent!");
                }
            }
        }
    }


}
