namespace LearningManagementSystem.Domain.Models.Report
{
    public class GroupReportModel
    {
        public string GroupName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateTime ReportCreatedTime { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, int?>>> Subjects { get; set; }
    }
}
