using LearningManagementSystem.Domain.Contextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace LearningManagementSystem.Core.Jobs
{
    public class BirthdayGreetingJob : IJob
    {
        private readonly ILogger<BirthdayGreetingJob> _logger;
        private readonly AppDbContext _context;
        public BirthdayGreetingJob(ILogger<BirthdayGreetingJob> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var today = DateTime.Today;
            var toGreat = _context.Users.AsNoTracking().Where(i => i.Birthday.Day.Equals(today.Day)
                                                                   && i.Birthday.Month.Equals(today.Month)).AsEnumerable();
            if (toGreat is not null && toGreat.Any())
            {
                foreach (var user in toGreat)
                {
                    _logger.LogInformation("Happy Birthday: {firstName} {lastName}!", user.FirstName, user.LastName);
                }
            }
            return Task.CompletedTask;
        }
    }
}
