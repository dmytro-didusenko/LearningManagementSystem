using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LearningManagementSystem.Domain.Entities
{
    public class Student
    {
        [Key]
        [ForeignKey("User")]
        public Guid Id { get; set; }
        public User User { get; set; } = null!;
        public Guid? GroupId { get; set; }
        public Group? Group { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
    }
}
