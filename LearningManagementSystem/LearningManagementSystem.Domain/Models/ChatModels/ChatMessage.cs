
namespace LearningManagementSystem.Domain.ChatModels
{
    public class ChatMessage
    {
        public string Sender { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
