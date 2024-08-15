using Serilog.Events;
using Serilog;
using TaskManagementSolution.Extensions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using Application.Services;

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

// Register the JsonStringLocalizerFactory
//builder.Services.AddSingleton<IStringLocalizerFactory>(provider => new JsonStringLocalizerFactory("Resources"));

builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.AddMvc().AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(JsonStringLocalizerFactory));
    {
        //var localizerType = typeof(JsonStringLocalizer<>).MakeGenericType(type);
        //return (IStringLocalizer)factory.Create(localizerType);
    };
});

// Register the open generic type directly
//builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(JsonStringLocalizer<>));

// Register the open generic type using a factory method
//builder.Services.AddSingleton(typeof(IStringLocalizer<>), provider =>
//{
//    var factory = provider.GetRequiredService<IStringLocalizerFactory>();
//    var resourcesPath = "Resources";
//    var localizerType = typeof(JsonStringLocalizer<>).MakeGenericType(typeof(object)); // Use object as a placeholder
//    return Activator.CreateInstance(localizerType, resourcesPath);
//});

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

app.MapControllers();

app.Run();