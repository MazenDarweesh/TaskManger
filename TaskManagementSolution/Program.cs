using Serilog.Events;
using Serilog;
using TaskManagementSolution.Extensions;
using Microsoft.Extensions.Options;
using Hangfire;
using StackExchange.Redis; 

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddAllServices(builder.Configuration);

// Add StackExchange Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    var configuration = ConfigurationOptions.Parse("redis-14713.c9.us-east-1-2.ec2.redns.redis-cloud.com:14713,password=loNFX6DI1B4bYnh9WIFuitOK8aUbfvlv");
    options.ConfigurationOptions = configuration;
});


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

app.UseHangfireDashboard("/dashboard");

app.MapControllers();

app.Run();
