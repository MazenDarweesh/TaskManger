using Serilog.Events;
using Serilog;
using TaskManagementSolution.Extensions;
using Microsoft.Extensions.Options;
using Application.Interfaces;
using MailKit.Net.Smtp;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

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

// Configure SMTP client
builder.Services.AddSingleton(new SmtpClient());


// Add services to the container.
builder.Services.AddAllServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()?.Value ?? new RequestLocalizationOptions();
app.UseRequestLocalization(localizationOptions);

app.UseMiddleware<LocalizationMiddleware>();

app.MapControllers();

app.Run();
