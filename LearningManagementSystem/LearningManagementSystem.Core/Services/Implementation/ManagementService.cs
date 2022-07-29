using AutoMapper;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.User;
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
                throw new NotFoundException(studentId);
            }

            if (student.GroupId is not null)
            {
                throw new BadRequestException("Student already has a group");
            }

            var group = await _context.Groups.SingleOrDefaultAsync(f => f.Id.Equals(groupId));
            if (group is null)
            {
                throw new NotFoundException(groupId);
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
                throw new NotFoundException(courseId);
            }

            var group = await _context.Groups.SingleOrDefaultAsync(f => f.Id.Equals(groupId) && f.IsActive.Equals(true));
            if (group is null)
            {
                throw new NotFoundException(groupId);
            }

            if (group.CourseId is not null)
            {
                throw new BadRequestException("Group already has a course!");
            }

            group.CourseId = courseId;
            _context.Groups.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task AddSubjectToCourse(Guid subjectId, Guid courseId)
        {
            var subject = await _context.Subjects.Include(i => i.Courses)
                .SingleOrDefaultAsync(s => s.Id.Equals(subjectId));

            if (subject is null)
            {
                throw new NotFoundException(subjectId);
            }

            var course = await _context.Courses.SingleOrDefaultAsync(s => s.Id.Equals(courseId));
            if (course is null)
            {
                throw new NotFoundException(courseId);
            }

            if (subject.Courses.Contains(course))
            {
                throw new BadRequestException("Course already has a subject");
            }
            subject.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task AddTeacherToSubject(Guid teacherId, Guid subjectId)
        {
            var teacher = await _context.Teachers
                .Include(i => i.Subject)
                .SingleOrDefaultAsync(s => s.Id.Equals(teacherId));

            if (teacher is null)
            {
                throw new NotFoundException(teacherId);
            }
            if (teacher.SubjectId is not null)
            {
                throw new BadRequestException("Teacher already has a subject");
            }

            var subject = await _context.Subjects.SingleOrDefaultAsync(s => s.Id.Equals(subjectId));
            if (subject is null)
            {
                throw new NotFoundException(subjectId);
            }

            teacher.SubjectId = subjectId;
            _context.Update(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StudentModel>> AddStudentsToGroupAsync(List<Guid> studentIds, Guid groupId)
        {
            var students = new List<Student>();
            foreach (var studentId in studentIds)
            {
                var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == studentId);

                if (student is null)
                    throw new NotFoundException($"Student with id [{studentId}] was not found.");

                if (student.GroupId is not null)
                    throw new BadRequestException($"Student with id [{studentId}] already has a group.");
                
                var group = await _context.Groups.FindAsync(groupId);

                if (group is null)
                    throw new NotFoundException($"Group with id [{groupId}] was not found.");

                student.GroupId = groupId;
                students.Add(student);
            }
            _context.Students.UpdateRange(students);
            await _context.SaveChangesAsync();
            return _mapper.Map<IEnumerable<StudentModel>>(students);
        }

        //public async Task RemoveStudentFromGroupAsync(Guid studentId, Guid groupId)
        //{
        //    var student = await _context.Students.FindAsync(studentId);
            
        //    if (student is null)
        //        throw new NotFoundException($"Student with id [{studentId}] was not found.");
            
        //    if (student.GroupId is null)
        //        throw new BadRequestException($"Student with id [{studentId}] is not in a group.");

        //    student.GroupId = null;
        //    _context.Students.Update(student);
        //    await _context.SaveChangesAsync();
        //}
    }
}
