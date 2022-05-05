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
            
        }
    }
}
