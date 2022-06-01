
using Microsoft.Extensions.Logging;
using Quartz;

namespace LearningManagementSystem.Core.Jobs
{
    public class TestJob : IJob
    {
        private readonly ILogger<TestJob> _logger;

        public TestJob(ILogger<TestJob> logger)
        {
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Delay(5000);
            _logger.LogCritical("JOB IS DONE");
        }
    }
}
