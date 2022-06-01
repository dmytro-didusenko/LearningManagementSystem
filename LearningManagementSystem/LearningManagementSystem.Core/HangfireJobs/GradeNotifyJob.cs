using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.MassTransitModels;
using MassTransit;

namespace LearningManagementSystem.Core.HangfireJobs
{
    public class GradeNotifyJob : IGradeNotifyJob
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publisher;

        public GradeNotifyJob(AppDbContext context, IPublishEndpoint publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        public async Task SendNotification(Guid studentId)
        {
            await Task.Delay(3000);
            var student = _context.Users.FirstOrDefault(f => f.Id.Equals(studentId));
            if (student == null)
                await _publisher.Publish(new ApiMessage()
                {
                    DeliveryMethod = DeliveryMethod.Email,
                    MessageType = MessageType.Information,
                    Text = $"Your work has been rated",
                    Receivers = new List<string>() { student.Email }
                });
        }
    }
}
