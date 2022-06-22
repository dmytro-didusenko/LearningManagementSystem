using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Hangfire;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.API.Hubs;
using LearningManagementSystem.API.Middlewares;
using LearningManagementSystem.Core.Jobs;
using LearningManagementSystem.Domain.Models.Options;
using LearningManagementSystem.Domain.Models.Report;
using LearningManagementSystem.Domain.Validators;
using MassTransit;
using Quartz;
using Swashbuckle.AspNetCore.SwaggerUI;

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

builder.Services.AddControllers().AddFluentValidation(cfg =>
{
    cfg.RegisterValidatorsFromAssemblyContaining<UserModelValidator>();
    cfg.DisableDataAnnotationsValidation = true;
    cfg.LocalizationEnabled = false;
});

builder.Services.AddDbContexts(builder.Configuration);
builder.Services.ConfigAutoMapper();
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.Configure<VisitingReportOptions>(builder.Configuration.GetSection("Reports").GetSection("VisitingReport"));

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
//});
//builder.Services.AddQuartzHostedService(cfg => cfg.WaitForJobsToComplete = true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.DocExpansion(DocExpansion.None));
}

//app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

app.MapHub<ChatHub>("/chat");


app.Run();