using LearningManagementSystem.Domain.MassTransitModels;
using MassTransit;
using Notifications.Services.Senders;

namespace Notifications.Services.Consumers
{
    public class ApiConsumer : IConsumer<ApiMessage>
    {
        IConfiguration _configuration;
        public ApiConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
 
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

            if (message.DeliveryMethod == DeliveryMethod.Email)
            {

                SendWithSendGrid.SendToEmail(message, _configuration).Wait();
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
