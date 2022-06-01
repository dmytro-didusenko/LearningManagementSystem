namespace LearningManagementSystem.Domain.Models.User
{
    public class StudentCreateModel
    {
        public Guid UserId { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
    }
}
