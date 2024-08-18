using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration; 
using Application.Interfaces;
using Persistence;
using Persistence.Repositories;
using Application.Services;
using Application.IServices;
using Infrastructure.Repositories;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using FluentValidation.AspNetCore;

namespace TaskManagementSolution.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddRepositories();
            services.AddServices();
            services.AddLoggingServices();
            services.AddControllerServices();
            services.AddSwaggerServices();
            services.AddAutoMapperServices();
            services.AddLocalizationServices();
            services.AddCustomMiddleware();
            services.AddDistributedCachingServices();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseName = configuration.GetConnectionString("InMemoryDatabase");
            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException(nameof(databaseName), "Database name cannot be null or empty.");
            }
            services.AddDbContext<TaskContext>(options =>
                options.UseInMemoryDatabase(databaseName));
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITaskService, TaskService>();
            return services;
        }

        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddLogging();
            return services;
        }

        public static IServiceCollection AddControllerServices(this IServiceCollection services)
        {
            services.AddControllers()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<StudentDTOValidator>()); // Use a known validator class
            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagementSolution", Version = "v1" });
                c.AddSecurityDefinition("Accept-Language", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Accept-Language",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Language header"
                });
                c.OperationFilter<AddLanguageHeaderOperationFilter>();
            });
            return services;
        }

        public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.Configure<RequestLocalizationOptions>(options =>
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
            return services;
        }

        public static IServiceCollection AddCustomMiddleware(this IServiceCollection services)
        {
            services.AddSingleton<LocalizationMiddleware>();
            return services;
        }

        public static IServiceCollection AddDistributedCachingServices(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            return services;
        }
    }
}
