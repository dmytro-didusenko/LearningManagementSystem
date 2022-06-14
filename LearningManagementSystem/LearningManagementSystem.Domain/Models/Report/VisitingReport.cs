namespace LearningManagementSystem.Domain.Models.Report
{
    public class VisitingReport
    {
        public string GroupName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateTime ReportCreatedTime { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Subjects { get; set; } = new();
    }
}
