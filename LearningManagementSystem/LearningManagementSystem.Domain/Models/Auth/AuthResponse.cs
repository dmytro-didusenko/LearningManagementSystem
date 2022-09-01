using System.Text.Json.Serialization;

namespace LearningManagementSystem.Domain.Models.Auth
{
    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = null!;
        public string Token { get; set; } = string.Empty;

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
