using LearningManagementSystem.Domain.Models;
using MassTransit;

namespace Notifications.Services
{
    public class BirthdayConsumer : IConsumer<GreetingMessage>
    {
        public Task Consume(ConsumeContext<GreetingMessage> context)
        {
            var msg = $"New message received\n" +
                      $"-{context.Message.FIO}\n" +
                      $"-{context.Message.Message}\n\n";
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}
