using AutoMapper;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Implementation;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.AutoMapper;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.EntityFrameworkCore;
using Quartz;

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
            services.AddScoped<IManagementService, ManagementService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IGroupService, GroupService>(); 
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IHomeTaskService, HomeTaskService>();

            return services;
        }

        public static void AddJobAndTrigger<T>(
            this IServiceCollectionQuartzConfigurator quartz,
            IConfiguration config)
            where T : IJob
        {
            string jobName = typeof(T).Name;

            var configKey = $"Quartz:{jobName}";
            var cronSchedule = config[configKey];

            if (string.IsNullOrEmpty(cronSchedule))
            {
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configKey}");
            }

            var jobKey = new JobKey(jobName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-trigger")
                .WithCronSchedule(cronSchedule));
        }
    }
}