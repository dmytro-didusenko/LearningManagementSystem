namespace LearningManagementSystem.Domain.Models.Auth
{
    public class AuthUserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
