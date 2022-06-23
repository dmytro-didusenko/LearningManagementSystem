using LearningManagementSystem.Domain.MassTransitModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.Services.Senders
{
    static internal class SendWithSendGrid
    {
        internal static async Task SendToEmail(ApiMessage message, IConfiguration configuration)
        {
            foreach (var item in message.Receivers)
            {
                var apiKey = configuration["SendGrid:Key"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(configuration["SendGrid:Email"]);
                var subject = $"Subject:{message.Subject}";
                var to = new EmailAddress($"{item}");
                var plainTextContent = $"{message.Text}";
                var htmlContent = string.Empty;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
        }
    }
}
