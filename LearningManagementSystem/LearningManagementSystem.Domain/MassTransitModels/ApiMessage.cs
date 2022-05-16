namespace LearningManagementSystem.Domain.MassTransitModels
{
    public class ApiMessage
    {
        public MessageType MessageType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        //TODO: Create IEnumerable<string> for "To"
        public string To { get; set; } = string.Empty;
        //public IEnumerable<string> Receivers { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}
