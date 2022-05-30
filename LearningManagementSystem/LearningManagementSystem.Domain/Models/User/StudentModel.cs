namespace LearningManagementSystem.Domain.Models.User
{
    public class StudentModel : UserModel
    {
        public string ContractNumber { get; set; } = string.Empty;
        public Guid? GroupId { get; set; }
    }
}
