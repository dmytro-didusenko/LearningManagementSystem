using LearningManagementSystem.Domain.MassTransitModels;
using LearningManagementSystem.Domain.Models;
using MassTransit;

namespace Notifications.Services.Consumers
{
    public class ApiConsumer : IConsumer<ApiMessage>
    {
        public Task Consume(ConsumeContext<ApiMessage> context)
        {
            var message = context.Message;
            switch (message.MessageType)
            {
                case MessageType.Information:
                    PrintMessageInConsole(message);
                    break;
                case MessageType.Error:
                    PrintMessageInConsole(message);
                    break;
            }

            return Task.CompletedTask;
        }

        private void PrintMessageInConsole(ApiMessage message)
        {
            Console.WriteLine(message.To);
            Console.WriteLine(message.Text);
        }
    }
}
