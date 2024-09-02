using Serilog.Events;
using Serilog;
using TaskManagementSolution.Extensions;
using Microsoft.Extensions.Options;

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
