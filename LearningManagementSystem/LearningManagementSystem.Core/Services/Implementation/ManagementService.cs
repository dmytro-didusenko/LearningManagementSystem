using AutoMapper;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public class ManagementService : IManagementService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ManagementService> _logger;

        public ManagementService(AppDbContext context,
            IMapper mapper,
            ILogger<ManagementService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddStudentToGroupAsync(Guid studentId, Guid groupId)
        {
            var student = await _context.Students.SingleOrDefaultAsync(f => f.Id.Equals(studentId));
            if (student is null)
            {
                throw new Exception("Student does not exist");
            }

            if (student.GroupId is not null)
            {
                throw new Exception("Student already has a group");
            }

            var group = await _context.Groups.SingleOrDefaultAsync(f => f.Equals(groupId));
            if (group is null)
            {
                throw new Exception("Group does not exist");
            }

            student.GroupId = groupId;
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task AddCourseToGroup(Guid courseId, Guid groupId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(f => f.Id.Equals(courseId));
            if (course is null)
            {
                throw new Exception("Course does not exist");
            }

            var group = await _context.Groups.SingleOrDefaultAsync(f => f.Id.Equals(groupId));
            if (course is null)
            {
                throw new Exception("Group does not exist");
            }

            if (group.CourseId is not null)
            {
                throw new Exception("Group already has a course!");
            }

            group.CourseId = courseId;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task AddSubjectToCourse(Guid subjectId, Guid courseId)
        {
            var subject = await _context.Subjects.Include(i=>i.Courses)
                .SingleOrDefaultAsync(s => s.Id.Equals(subjectId));

            if (subject is null)
            {
                throw new Exception("Subject does not exist");
            }

            var course = await _context.Courses.SingleOrDefaultAsync(s => s.Id.Equals(courseId));
            if (course is null)
            {
                throw new Exception("Course does not exist");
            }

            if (subject.Courses.Contains(course))
            {
                throw new Exception("Course already has a subject");
            }
            subject.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

    }
}
