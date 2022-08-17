using System.Text.Json.Serialization;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.API.Filters;
using LearningManagementSystem.API.Hubs;
using LearningManagementSystem.API.Middlewares;
using LearningManagementSystem.Domain.Models.Options;
using LearningManagementSystem.Domain.Validators;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerUI;
using AllowAnonymousAttribute = LearningManagementSystem.API.Attributes.AllowAnonymousAttribute;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddHangfire((provider, cfg) =>
{
    cfg.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireConnection"));
    cfg.UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings();
});
builder.Services.AddHangfireServer();

builder.Services.AddSwagger();

builder.Services.AddDbContexts(builder.Configuration);
builder.Services.ConfigAutoMapper();
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

//Options
builder.Services.Configure<VisitingReportOptions>(builder.Configuration.GetSection("Reports").GetSection("VisitingReport"));
builder.Services.Configure<JwtSettingsOptions>(builder.Configuration.GetSection("JWTSettings"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.SetIsOriginAllowed((host) => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddMvc(options =>
    {
        options.Filters.Add<ValidationFilter>(1);
        options.Filters.Add<AllowAnonymousAttribute>();
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddFluentValidation(cfg =>
{
    cfg.RegisterValidatorsFromAssemblyContaining<UserModelValidator>();
    cfg.DisableDataAnnotationsValidation = true;
    cfg.LocalizationEnabled = false;
});


//configuring MassTransit
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, x) =>
    {
        x.Host(new Uri(builder.Configuration["RabbitMQ:Uri"]));
    });
});
//Adding Quartz
//builder.Services.AddQuartz(cfg =>
//{
//    cfg.UseMicrosoftDependencyInjectionJobFactory();
//    cfg.AddJobAndTrigger<BirthdayGreetingJob>(builder.Configuration);
//    cfg.AddJobAndTrigger<CourseStartingJob>(builder.Configuration);
//    cfg.AddJobAndTrigger<HomeTaskNotificationJob>(builder.Configuration);
//    cfg.AddJobAndTrigger<CertificateJob>(builder.Configuration);
//});
//builder.Services.AddQuartzHostedService(cfg => cfg.WaitForJobsToComplete = true);

//add DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.DocExpansion(DocExpansion.None));
}
app.UseSwagger();
app.UseSwaggerUI(x => x.DocExpansion(DocExpansion.None));

//app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseMiddleware<AuthMiddleware>();

//app.UseHangfireDashboard();

app.UseCors();

app.MapControllers();

app.MapHub<ChatHub>("/hubs/chat");

app.MapHub<NotificationHub>("/notification");

app.Run();

