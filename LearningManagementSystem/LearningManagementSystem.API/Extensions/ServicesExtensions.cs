using AutoMapper;
using LearningManagementSystem.API.SignalRServices;
using LearningManagementSystem.Core.HangfireJobs;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Services.Implementation;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.AutoMapper;
using LearningManagementSystem.Domain.Contextes;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LMS",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    //Type = SecuritySchemeType.ApiKey,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
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
            services.AddScoped<ILearningService, LearningService>();
            services.AddScoped<ITestingService, TestingService>();
            services.AddScoped<IGradeNotifyJob, GradeNotifyJob>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddSingleton<IUserConnectionService, UserConnectionService>();
            services.AddHostedService(sp => (SignalRNotificationService)sp.GetService<INotificationSink>());
            services.AddSingleton<INotificationSink, SignalRNotificationService>();
            
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