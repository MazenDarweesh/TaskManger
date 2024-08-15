using Serilog.Events;
using Serilog;
using TaskManagementSolution.Extensions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using Application.Services;
using Microsoft.Extensions.Logging;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddLocalization();
builder.Services.AddSingleton<LocalizationMiddleware>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

// Add services to the container.
builder.Services.AddAllServices(builder.Configuration);

// Register the JsonStringLocalizerFactory
//builder.Services.AddSingleton<IStringLocalizerFactory>(provider => new JsonStringLocalizerFactory("Resources"));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("ar")
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.SetDefaultCulture("en");
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
//app.UseStaticFiles();
app.UseMiddleware<LocalizationMiddleware>();

app.MapControllers();

app.Run();
