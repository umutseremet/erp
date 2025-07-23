using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;
using ERP.Infrastructure.Repositories;
using ERP.Infrastructure.ExternalServices.Redmine;
using ERP.Infrastructure.ExternalServices.Redmine.Configuration;
using ERP.Infrastructure.ExternalServices.Email;
using ERP.Infrastructure.ExternalServices.Email.Models;
using ERP.Infrastructure.ExternalServices.SMS;
using ERP.Infrastructure.ExternalServices.SMS.Models;
using ERP.Infrastructure.BackgroundServices;
using ERP.Infrastructure.Caching;
using ERP.Infrastructure.Logging;

namespace ERP.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IFuelTransactionRepository, FuelTransactionRepository>();
            services.AddScoped<IVehicleMaintenanceRepository, VehicleMaintenanceRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IInsurancePolicyRepository, InsurancePolicyRepository>();
            services.AddScoped<IVehicleLocationHistoryRepository, VehicleLocationHistoryRepository>();
            services.AddScoped<IVehicleNotificationRepository, VehicleNotificationRepository>();

            // External Services - HTTP Client eklemeleri
            services.AddHttpClient();
            services.AddHttpClient<IRedmineService, RedmineService>(client =>
            {
                var redmineSettings = configuration.GetSection("Redmine").Get<RedmineSettings>();
                if (redmineSettings != null)
                {
                    client.BaseAddress = new Uri(redmineSettings.BaseUrl);
                    client.DefaultRequestHeaders.Add("X-Redmine-API-Key", redmineSettings.ApiKey);
                    client.Timeout = TimeSpan.FromSeconds(redmineSettings.TimeoutSeconds);
                }
            });

            services.AddHttpClient<ISmsService, SmsService>(client =>
            {
                var smsSettings = configuration.GetSection("SMS").Get<SmsSettings>();
                if (smsSettings != null && !string.IsNullOrEmpty(smsSettings.ApiEndpoint))
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                }
            });

            services.AddScoped<IEmailService, EmailService>();

            // Background Services
            services.AddHostedService<NotificationService>();
            services.AddHostedService<MaintenanceReminderService>();
            services.AddHostedService<InsuranceExpiryService>();
            services.AddHostedService<LocationTrackingService>();

            // Caching
            services.AddMemoryCache();
            services.TryAddScoped<ICacheService, MemoryCacheService>();

            // Redis Cache (if configured)
            var redisConnection = configuration.GetConnectionString("Redis");
            if (!string.IsNullOrEmpty(redisConnection))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnection;
                });
                services.AddScoped<ICacheService, RedisCacheService>();
            }

            // Logging
            services.AddScoped<ICustomLogger, DatabaseLogger>();

            // Configuration
            services.Configure<RedmineSettings>(configuration.GetSection("Redmine"));
            services.Configure<EmailSettings>(configuration.GetSection("Email"));
            services.Configure<SmsSettings>(configuration.GetSection("SMS"));

            return services;
        }
    }
}