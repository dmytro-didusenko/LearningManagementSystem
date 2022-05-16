namespace LearningManagementSystem.Domain.MassTransitModels
{
    public class ApiMessage
    {
        public MessageType MessageType { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public string To { get; set; } = null!;
        public string Text { get; set; } = null!;
    }
}
