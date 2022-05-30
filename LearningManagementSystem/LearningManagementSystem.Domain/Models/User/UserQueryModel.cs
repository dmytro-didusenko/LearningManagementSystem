namespace LearningManagementSystem.Domain.Models
{
    public class UserQueryModel
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthdayGreaterThan { get; set; }
        public DateTime? BirthdayLessThan { get; set; }

    }
}
