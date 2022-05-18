using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Domain.Entities
{
    public class Teacher
    {
        [Key]
        [ForeignKey("User")]
        public Guid Id { get; set; }
        public Guid? SubjectId { get; set; }
        public User User { get; set; } = null!;
        public Subject? Subject { get; set; } = null!;
        public string Position { get; set; } = string.Empty;
    }
}
