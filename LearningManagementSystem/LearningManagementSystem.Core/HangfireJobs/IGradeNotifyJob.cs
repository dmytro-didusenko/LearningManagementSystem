namespace LearningManagementSystem.Core.HangfireJobs
{
    public interface IGradeNotifyJob
    {
        public Task SendNotification(Guid studentId);
    }
}
