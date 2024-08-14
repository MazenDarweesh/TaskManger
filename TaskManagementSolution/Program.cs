using Serilog.Events;
using Serilog;
using Application.Interfaces;
using Persistence;
using Persistence.Repositories;
using Application.Services;
using Application.IServices;
using Infrastructure.Repositories;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add services to the container
builder.Services.AddDbContext<TaskContext>(options =>
    options.UseInMemoryDatabase("TaskList")); //need to be in the appstengs.json

// Configure RabbitMQ settings
var rabbitMQSettings = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
if (rabbitMQSettings == null)
{
    throw new InvalidOperationException("RabbitMQSettings section is missing in the configuration.");
}
builder.Services.AddSingleton(rabbitMQSettings);

// Register RabbitMQ producer and consumer
builder.Services.AddSingleton<IMessagePublisher, RabbitMQProducer>();
builder.Services.AddHostedService<RabbitMQConsumer>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SMTP client
builder.Services.AddSingleton(new SmtpClient());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.MapControllers();

app.Run();