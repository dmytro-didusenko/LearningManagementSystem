using LearningManagementSystem.Domain.MassTransitModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications.Services.Senders
{
    public static class SendWithSendGrid
    {
        public static async Task SendToEmail(ApiMessage message, IConfiguration configuration)
        {
            var apiKey = configuration["SendGrid:Key"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(configuration["SendGrid:Email"]);

            foreach (var item in message.Receivers)
            {
                var subject = $"Subject:{message.Subject}";
                var to = new EmailAddress($"{item}");
                var plainTextContent = $"{message.Text}";
                var htmlContent = string.Empty;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                Console.WriteLine($"Message to {item}: isSuccessStatusCode:{response.IsSuccessStatusCode}, StatusCode:{response.StatusCode}");
            }
        }
    }
}
