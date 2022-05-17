namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IManagementService
    {
        public Task AddStudentToGroupAsync(Guid studentId, Guid groupId);
        public Task AddCourseToGroup(Guid courseId, Guid groupId);
        public Task AddSubjectToCourse(Guid subjectId, Guid courseId);
    }
}
