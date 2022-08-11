using LearningManagementSystem.Domain.Entities;

namespace LearningManagementSystem.Domain.Models.Auth
{
    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
