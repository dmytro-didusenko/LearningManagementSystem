using LearningManagementSystem.Domain.MassTransitModels;
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
                default:
                    PrintMessageInConsole(message);
                    break;
            }

            return Task.CompletedTask;
        }

        private void PrintMessageInConsole(ApiMessage message)
        {
            Console.WriteLine("\n");
            Console.WriteLine(message.Subject);
            foreach (var receiver in message.Receivers)
            {
                Console.WriteLine($"To: {receiver}");
            }
            Console.WriteLine(message.Text);
            Console.WriteLine("\n");
        }
    }
}
