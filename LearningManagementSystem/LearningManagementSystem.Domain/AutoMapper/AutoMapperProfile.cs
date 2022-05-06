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
            CreateMap<Student, StudentModel>().ReverseMap();
            CreateMap<Group, GroupModel>().ForMember(m=>m.StudentsIds,
                opt=>
                    opt.MapFrom(req=>req.Students.Select(s=>s.Id))).ReverseMap();
        }
    }
}
