namespace LearningManagementSystem.Domain.Models.Group
{
    public class GroupCreateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartEducation { get; set; }
        public DateTime EndEducation { get; set; }
    }
}
