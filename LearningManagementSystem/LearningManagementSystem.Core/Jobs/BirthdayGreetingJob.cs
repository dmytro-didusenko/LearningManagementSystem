using System.Text;
using LearningManagementSystem.Core.RabbitMqServices;
using LearningManagementSystem.Domain.Contextes;
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
        private readonly IMessageProducer _messageProducer;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly string _queue;

        public BirthdayGreetingJob(AppDbContext context, ILogger<BirthdayGreetingJob> logger, 
            IMessageProducer messageProducer,
            IConfiguration configuration)
        {
            _logger = logger;
            _messageProducer = messageProducer;
            _configuration = configuration;
            _context = context;
            _queue = _configuration["RabbitMQ:Queues:BirthdayQueue"];
        }

        public Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.Today;
            var toGreat = _context.Users.AsNoTracking().Where(i => i.Birthday.Day.Equals(today.Day)
                                                                   && i.Birthday.Month.Equals(today.Month)).AsEnumerable();
            if (toGreat is not null && toGreat.Any())
            {
                //Create RabbitMQMessage Producer here?
                foreach (var user in toGreat)
                {
                    var message = $"Happy Birthday: {user.FirstName} {user.LastName}!";
                    _messageProducer.SendMessage(_queue , message);
                }
            }
            return Task.CompletedTask;
        }
    }


}
