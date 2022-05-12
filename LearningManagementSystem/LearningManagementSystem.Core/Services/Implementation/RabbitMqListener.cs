using System;
using System.Text;
using System.Xml;
using LearningManagementSystem.Core.RabbitMqServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace LearningManagementSystem.Core.Services.Implementation
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string _queue;

        public RabbitMqListener(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _queue = _configuration["RabbitMQ:Queues:BirthdayQueue"];
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();
                consumer.ReceiveMessage<string>(_queue, item =>
                {
                    Console.WriteLine(item);
                });
            }
            return Task.CompletedTask;
        }

    }
}
