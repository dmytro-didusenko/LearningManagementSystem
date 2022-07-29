using LearningManagementSystem.API.Hubs;
using LearningManagementSystem.API.SignalRServices;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.NotificationMessage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public interface INotificationSink
    {
        ValueTask PushAsync(NotificationMessage notification);
    }

    public class SignalRNotificationService : BackgroundService, INotificationSink
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SignalRNotificationService> _logger;
        private readonly IUserConnectionService _userConnectionService;
        private readonly Channel<NotificationMessage> _channel;

        public ValueTask PushAsync(NotificationMessage notification) => _channel.Writer.WriteAsync(notification);

        public SignalRNotificationService(
            IServiceProvider serviceProvider,
            ILogger<SignalRNotificationService> logger,
            IUserConnectionService userConnectionService
        )
        {
            _channel = Channel.CreateUnbounded<NotificationMessage>();
            _serviceProvider = serviceProvider;
            _logger = logger;
            _userConnectionService = userConnectionService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Local
            while (true)
            {
                try
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        return;
                    }


                    var message = await _channel.Reader.ReadAsync(stoppingToken);

                    using var scope = _serviceProvider.CreateScope();

                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    var userConnections = _userConnectionService.GetUserConnections(message.UserId);

                    _logger.LogInformation($"Sending channel notification '{message.Text}' to ");

                    await hub.Clients.Clients(userConnections).SendAsync("ShowNotification", message, stoppingToken);

                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error in notification service.");
                }
            }
        }
    }
}
