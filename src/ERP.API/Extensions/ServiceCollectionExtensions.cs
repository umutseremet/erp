using AutoMapper;
using ERP.Application.Interfaces;
using ERP.Application.Interfaces.External;
using ERP.Application.Interfaces.Services;
using ERP.Application.Services;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;
using ERP.Infrastructure.External.Redmine;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application Services
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVehicleLocationService, VehicleLocationService>();
            services.AddScoped<IVehicleAssignmentService, VehicleAssignmentService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IFuelService, FuelService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVehicleLocationHistoryRepository, VehicleLocationHistoryRepository>();
            services.AddScoped<IVehicleAssignmentRepository, VehicleAssignmentRepository>();
            services.AddScoped<IVehicleMaintenanceRepository, VehicleMaintenanceRepository>();
            services.AddScoped<IVehicleNotificationRepository, VehicleNotificationRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IFuelRecordRepository, FuelRecordRepository>();
            services.AddScoped<ISystemLogRepository, SystemLogRepository>();

            // External Services
            services.Configure<RedmineSettings>(configuration.GetSection("Redmine"));
            services.AddScoped<IRedmineApiService, RedmineApiService>();

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(VehicleMappingProfile));

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateVehicleDtoValidator>();

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}