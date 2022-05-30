using LearningManagementSystem.Domain.Entities;

namespace LearningManagementSystem.Domain.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string? About { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
