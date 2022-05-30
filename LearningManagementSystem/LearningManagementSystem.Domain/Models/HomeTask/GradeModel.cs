using LearningManagementSystem.Domain.Entities;

namespace LearningManagementSystem.Domain.Models.HomeTask
{
    public class GradeModel
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
