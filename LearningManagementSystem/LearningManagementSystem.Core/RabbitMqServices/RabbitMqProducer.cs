using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace LearningManagementSystem.Core.RabbitMqServices
{
    public class RabbitMqProducer : IMessageProducer, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RabbitMqProducer> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqProducer(IConfiguration configuration, ILogger<RabbitMqProducer> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connection = CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage<T>(string queue, T data)
        {
            var serialized = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(serialized);
            _channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
            _logger.LogInformation("Message has been successfully sent");
        }

        private IConnection CreateConnection()
        {
            var uri = new Uri(_configuration["RabbitMq:Uri"]);

            var factory = new ConnectionFactory()
            {
                Uri = uri
            };
            return factory.CreateConnection();
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
            _logger.LogInformation("RabbitMQ Message Producer is disposed!");
        }
    }
}