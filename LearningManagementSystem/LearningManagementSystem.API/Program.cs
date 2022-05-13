using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.API.Middlewares;
using LearningManagementSystem.Core.Helpers;
using LearningManagementSystem.Core.Jobs;
using MassTransit;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContexts(builder.Configuration);
builder.Services.ConfigAutoMapper();
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configuring MassTransit
builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, x) =>
    {
        x.Host(new Uri(builder.Configuration["RabbitMQ:Uri"]));
    });
});
//Adding Quartz
builder.Services.AddQuartz(cfg =>
{
    cfg.UseMicrosoftDependencyInjectionJobFactory();
    cfg.AddJobAndTrigger<BirthdayGreetingJob>(builder.Configuration);
    cfg.AddJobAndTrigger<CourseStartingJob>(builder.Configuration);
});
builder.Services.AddQuartzHostedService(cfg => cfg.WaitForJobsToComplete = true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

