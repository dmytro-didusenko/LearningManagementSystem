
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearningManagementSystem.Core.RabbitMqServices
{
    public class RabbitMqConsumer : IMessageConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumer(IConfiguration configuration, ILogger<RabbitMqConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration["RabbitMQ:Uri"])
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public  void ReceiveMessage<T>(string queue, Action<T> onMessage)
        {
            _channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var item = JsonSerializer.Deserialize<T>(content);
                _logger.LogCritical("Received new Message: {content}", item);
                onMessage(item);
            };
            _channel.BasicConsume(queue, true, consumer);
        }
    }
}