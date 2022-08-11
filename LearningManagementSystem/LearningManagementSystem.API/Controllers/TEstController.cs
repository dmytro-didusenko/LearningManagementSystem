﻿using Hangfire;
using LearningManagementSystem.Core.HangfireJobs;
using LearningManagementSystem.Core.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

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
            jobClient.Enqueue<IGradeNotifyJob>(job => job.SendNotification(id));
            return Ok();
        }

    }
}
