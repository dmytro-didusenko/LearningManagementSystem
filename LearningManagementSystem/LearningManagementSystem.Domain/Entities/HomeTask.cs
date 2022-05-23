using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningManagementSystem.Domain.Entities
{
    public class HomeTask 
    {
        [Key]
        [ForeignKey("Topic")]
        public Guid Id { get; set; }
        public Topic Topic { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DatePlannedStart { get; set; }
        public DateTime DateOfExpiration { get; set; }
        public ICollection<TaskAnswer>? TaskAnswers { get; set; }
    }
}
