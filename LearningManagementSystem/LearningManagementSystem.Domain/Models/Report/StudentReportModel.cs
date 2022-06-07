namespace LearningManagementSystem.Domain.Models.Report
{
    public class StudentReportModel
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public Dictionary<string, IEnumerable<TopicInfoModel>>? Subjects { get; set; } = null!;
        public DateTime ReportCreatedTime { get; set; }
    }
    public class TopicInfoModel
    {
        public string TopicName { get; set; } = string.Empty;
        public int? Grade { get; set; }
    }
}
