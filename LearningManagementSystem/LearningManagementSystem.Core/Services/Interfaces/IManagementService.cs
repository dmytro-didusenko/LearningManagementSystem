namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IManagementService
    {
        public Task AddStudentToGroupAsync(Guid studentId, Guid groupId);
        public Task AddStudentsToGroupAsync(List<Guid> studentIds, Guid groupId);
        //public Task RemoveStudentFromGroupAsync(Guid studentId, Guid groupId);
        public Task AddCourseToGroup(Guid courseId, Guid groupId);
        public Task AddSubjectToCourse(Guid subjectId, Guid courseId);
        public Task AddTeacherToSubject(Guid teacherId, Guid subjectId);
    }
}