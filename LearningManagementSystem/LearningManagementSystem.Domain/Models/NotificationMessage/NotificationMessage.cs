using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Models.NotificationMessage
{
    public class NotificationMessage
    {
        public Guid UserId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public DateTime SendingDate { get; set; } = DateTime.Now;
        public NotificationMessage(NotificationMessageType type, string text)
        {
            Type = type.ToString();
            Text = text;
        }


        public NotificationMessage()
        {
            SendingDate = DateTime.Now;
        }
    }
}
