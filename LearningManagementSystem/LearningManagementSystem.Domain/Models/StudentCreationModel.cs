namespace LearningManagementSystem.Domain.Models
{
    public class StudentCreationModel
    {
        public Guid UserId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
    }
}
