using LearningManagementSystem.Core.Services.Implementation;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.MassTransitModels;
using LearningManagementSystem.Domain.Models.NotificationMessage;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace LearningManagementSystem.Core.HangfireJobs
{
    public class GradeNotifyJob : IGradeNotifyJob
    {
        private readonly AppDbContext _context;
        private readonly INotificationSink notificationSink;

        public GradeNotifyJob(AppDbContext context, INotificationSink notificationSink)
        {
            _context = context;
            this.notificationSink = notificationSink;
        }

        public async Task SendNotification(Guid studentId)
        {
            var student = _context.Users.FirstOrDefault(f => f.Id.Equals(studentId));
            if (student != null)
            {
                var message = new NotificationMessage() { UserId = studentId, Text = "You has been graduated", Type=NotificationMessageType.success.ToString()};

                await notificationSink.PushAsync(message);
            }
        
        }

    }

}
