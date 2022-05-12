using System.Text;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.API.Middlewares;
using LearningManagementSystem.Core.Jobs;
using LearningManagementSystem.Core.RabbitMqServices;
using Quartz;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContexts(builder.Configuration);
builder.Services.ConfigAutoMapper();
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMessageProducer, RabbitMqProducer>();
builder.Services.AddHostedService<RabbitMqConsumer>();

//Adding Quartz
builder.Services.AddQuartz(cfg =>
{
    cfg.UseMicrosoftDependencyInjectionJobFactory();
    cfg.AddJobAndTrigger<BirthdayGreetingJob>(builder.Configuration);
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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

