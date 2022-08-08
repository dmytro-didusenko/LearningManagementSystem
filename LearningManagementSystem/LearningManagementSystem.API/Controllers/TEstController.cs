using Hangfire;
using LearningManagementSystem.Core.HangfireJobs;
using LearningManagementSystem.Core.Services.Implementation;
using LearningManagementSystem.Domain.Models.NotificationMessage;
using LearningManagementSystem.Domain.Models.Options;
using MassTransit.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TEstController : ControllerBase
    {
        private readonly INotificationSink notificationSink;
        private readonly IBackgroundJobClient jobClient;

        public TEstController(INotificationSink notificationSink, IBackgroundJobClient jobClient)
        {
            this.notificationSink = notificationSink;
            this.jobClient = jobClient;
        }


        [HttpGet("id")]
        public async Task<IActionResult> Get(Guid id)
        {

            jobClient.Enqueue<IGradeNotifyJob>(job =>job.SendNotification(id));
            return Ok();
        }

    }
}
