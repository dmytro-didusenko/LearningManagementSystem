using System.Text.Json.Serialization;

namespace LearningManagementSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string? About { get; set; }
        public bool IsActive { get; set; } = true;
        public Gender Gender { get; set; }
        public ICollection<Document>? Document { get; set; }
        public Guid? RoleId { get; set; }
        public Role? Role { get; set; } = null!;
        [JsonIgnore] public string PasswordHash { get; set; } = string.Empty!;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = null!;
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
