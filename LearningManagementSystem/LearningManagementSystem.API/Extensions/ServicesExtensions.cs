using AutoMapper;
using LearningManagementSystem.Core.Services.Implementation;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.AutoMapper;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.API.Extensions
{
    public static class ServicesExtensions
    {

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration cfg)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cfg.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection ConfigAutoMapper(this IServiceCollection services)
        {
            var mappperCfg = new MapperConfiguration(c =>
            {
                c.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappperCfg.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
