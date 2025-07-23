using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ERP.Core.Interfaces;

namespace ERP.Infrastructure.BackgroundServices
{
    public class LocationTrackingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LocationTrackingService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(30);

        public LocationTrackingService(IServiceProvider serviceProvider, ILogger<LocationTrackingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupOldLocationsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cleaning up old locations");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }

        private async Task CleanupOldLocationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var vehicles = await unitOfWork.Vehicles.GetAllAsync();
            var cutoffDate = DateTime.UtcNow.AddDays(-90); // 90 günden eski kayýtlarý sil

            foreach (var vehicle in vehicles)
            {
                try
                {
                    await unitOfWork.VehicleLocationHistories.DeleteOldLocationsAsync(vehicle.Id, cutoffDate);
                    _logger.LogInformation($"Cleaned up old location records for vehicle {vehicle.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to cleanup location records for vehicle {vehicle.Id}");
                }
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}