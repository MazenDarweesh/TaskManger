using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Persistence;
using Persistence.Repositories;
using Application.Services;
using Application.IServices;
using Infrastructure.Repositories;

using AutoMapper;



namespace TaskManagementSolution.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<TaskContext>(options =>
                options.UseInMemoryDatabase("TaskList")); //need to be in the appstengs.json
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
            services.AddControllers();
            return services;
        }

        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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

        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddRepositories();
            services.AddServices();
            services.AddLoggingServices();
            services.AddControllerServices();
            services.AddSwaggerServices();
            services.AddAutoMapperServices();
            return services;
        }
    }
}
