using AutoMapper;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Extensions;
using LearningManagementSystem.Domain.Models.Course;
using LearningManagementSystem.Domain.Models.Group;
using LearningManagementSystem.Domain.Models.HomeTask;
using LearningManagementSystem.Domain.Models.Options;
using LearningManagementSystem.Domain.Models.Subject;
using LearningManagementSystem.Domain.Models.Testing;
using LearningManagementSystem.Domain.Models.Topic;
using LearningManagementSystem.Domain.Models.User;
using StudentAnswer = LearningManagementSystem.Domain.Entities.StudentAnswer;

namespace LearningManagementSystem.Domain.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<Group, GroupCreateModel>().ReverseMap();
            CreateMap<Group, GroupModel>().ReverseMap();
            CreateMap<Document, DocumentModel>().ReverseMap();
            CreateMap<TaskAnswer, TaskAnswerModel>().ReverseMap();

            var splitter = ":";
            CreateMap<VisitingReportOptions, VisitingReportModel>()
                .ForMember(m => m.CourseCell, opt =>
                    opt.MapFrom(f => f.CourseCell.ToTuple(splitter)))
                .ForMember(m => m.GroupCell, opt =>
                    opt.MapFrom(f => f.GroupCell.ToTuple(splitter)))
                .ForMember(m => m.DateCell, opt =>
                    opt.MapFrom(f => f.DateCell.ToTuple(splitter)))
                .ForMember(m => m.StudentsStartCell, opt =>
                    opt.MapFrom(f => f.StudentsStartCell.ToTuple(splitter)))
                .ForMember(m => m.SubjectCell, opt =>
                    opt.MapFrom(f => f.SubjectCell.ToTuple(splitter)))
                .ForMember(m => m.TopicsStartCell, opt =>
                    opt.MapFrom(f => f.TopicsStartCell.ToTuple(splitter)));


            CreateMap<StudentAnswer, StudentAnswerModel>().ReverseMap();

            CreateMap<Question, QuestionPassingModel>().ReverseMap();
            CreateMap<Answer, AnswerPassingModel>().ReverseMap();

            CreateMap<Test, TestModel>().ReverseMap();
            CreateMap<Question, QuestionCreateModel>()
                .ForMember(m=>m.Answers, opt=>
                    opt.MapFrom(f=>f.Answers)).ReverseMap();
            CreateMap<Answer, AnswerCreateModel>().ReverseMap();

            CreateMap<Topic, TopicCreateModel>().ReverseMap();

            CreateMap<Grade, GradeModel>().ReverseMap();

            CreateMap<Topic, TopicModel>()
                .ForMember(m=>m.HomeTaskModel, opt=>
                    opt.MapFrom(f=>f.HomeTask)).ReverseMap();

            CreateMap<HomeTask, HomeTaskModel>()
                .ForMember(m => m.TaskAnswersIds, opt =>
                    opt.MapFrom(f => f.TaskAnswers.Select(s => s.Id))).ReverseMap();

            CreateMap<HomeTask, HomeTaskCreateModel>()
                .ForMember(m=>m.TopicId, opt=>
                    opt.MapFrom(f=>f.TopicId)).ReverseMap();

            CreateMap<Course, CourseModel>()
               .ReverseMap();

            CreateMap<TeacherCreateModel, Teacher>()
                .ForMember(m=>m.Id, opt=>
                    opt.MapFrom(f=>f.UserId)).ReverseMap();

            CreateMap<Teacher, TeacherModel>()
                .ForMember(m => m.UserName, opt =>
                    opt.MapFrom(f => f.User.UserName))
                .ForMember(m => m.FirstName, opt =>
                    opt.MapFrom(f => f.User.FirstName))
                .ForMember(m => m.LastName, opt =>
                    opt.MapFrom(f => f.User.LastName))
                .ForMember(m => m.Birthday, opt =>
                    opt.MapFrom(f => f.User.Birthday))
                .ForMember(m => m.Email, opt =>
                    opt.MapFrom(f => f.User.Email))
                .ForMember(m => m.About, opt =>
                    opt.MapFrom(f => f.User.About)).ReverseMap();

            CreateMap<Subject, SubjectModel>()
                .ForMember(m => m.CoursesIds, opt =>
                    opt.MapFrom(f => f.Courses.Select(s => s.Id)))
                .ForMember(m => m.TeachersIds, opt =>
                    opt.MapFrom(f => f.Teachers.Select(s => s.Id)))
                .ForMember(m=>m.TopicsIds, opt=>
                    opt.MapFrom(f=>f.Topics.Select(s=>s.Id)))
                .ReverseMap();

            CreateMap<Student, StudentModel>()
                .ForMember(m => m.UserName, opt =>
                    opt.MapFrom(f => f.User.UserName))
                .ForMember(m => m.FirstName, opt =>
                    opt.MapFrom(f => f.User.FirstName))
                .ForMember(m => m.LastName, opt =>
                    opt.MapFrom(f => f.User.LastName))
                .ForMember(m => m.Birthday, opt =>
                    opt.MapFrom(f => f.User.Birthday))
                .ForMember(m => m.Email, opt =>
                    opt.MapFrom(f => f.User.Email))
                .ForMember(m => m.About, opt =>
                    opt.MapFrom(f => f.User.About)).ReverseMap();
        }
    }


}
