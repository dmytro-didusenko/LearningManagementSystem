using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearningManagementSystem.Core.RabbitMqServices
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _queue;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumer(ILogger<RabbitMqConsumer> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_configuration["RabbitMQ:Uri"])
            };

            _queue = _configuration["RabbitMQ:Queues:BirthdayQueue"];
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                Dispose();
            }
            _channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                _logger.LogCritical("Received new Message: {content}", content);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_queue, false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
