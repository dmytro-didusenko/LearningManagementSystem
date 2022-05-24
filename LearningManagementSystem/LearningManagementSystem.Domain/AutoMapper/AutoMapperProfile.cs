using AutoMapper;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Domain.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<Group, GroupCreationModel>().ReverseMap();
            CreateMap<Group, GroupModel>().ReverseMap();
            CreateMap<Document, DocumentModel>().ReverseMap();
            CreateMap<TaskAnswer, TaskAnswerModel>().ReverseMap();

            CreateMap<Topic, TopicCreateModel>().ReverseMap();

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

            CreateMap<TeacherCreationModel, Teacher>()
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
