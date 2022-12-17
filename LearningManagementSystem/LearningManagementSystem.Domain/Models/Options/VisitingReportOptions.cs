
namespace LearningManagementSystem.Domain.Models.Options
{
    public class VisitingReportOptions
    {
        public string GroupCell { get; set; } = string.Empty;
        public string CourseCell { get; set; } = string.Empty;
        public string DateCell { get; set; } = string.Empty;
        public string SubjectCell { get; set; } = string.Empty;
        public string StudentsStartCell { get; set; } = string.Empty;
        public string TopicsStartCell { get; set; } = string.Empty; 
    }

    public class VisitingReportModel
    {
        public (int row, int col) GroupCell { get; set; } 
        public (int row, int col) CourseCell { get; set; }
        public (int row, int col) DateCell { get; set; } 
        public (int row, int col) SubjectCell { get; set; } 
        public (int row, int col) StudentsStartCell { get; set; }
        public (int row, int col) TopicsStartCell { get; set; } 
    }
}
