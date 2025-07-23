using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ERP.Core.Interfaces;
using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Infrastructure.BackgroundServices
{
    public class MaintenanceReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MaintenanceReminderService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(6);

        public MaintenanceReminderService(IServiceProvider serviceProvider, ILogger<MaintenanceReminderService> logger)
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
                    await CheckMaintenanceRemindersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking maintenance reminders");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }

        private async Task CheckMaintenanceRemindersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var upcomingMaintenances = await unitOfWork.VehicleMaintenances.GetUpcomingMaintenanceAsync();

            foreach (var maintenance in upcomingMaintenances)
            {
                try
                {
                    var daysUntilMaintenance = (maintenance.ScheduledDate - DateTime.UtcNow).Days;

                    if (daysUntilMaintenance <= 7 && daysUntilMaintenance > 0)
                    {
                        await CreateMaintenanceReminderNotificationAsync(maintenance, daysUntilMaintenance, unitOfWork);
                    }
                    else if (daysUntilMaintenance <= 0)
                    {
                        await CreateOverdueMaintenanceNotificationAsync(maintenance, unitOfWork);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to create maintenance reminder for {maintenance.Id}");
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        private async Task CreateMaintenanceReminderNotificationAsync(VehicleMaintenance maintenance, int daysUntil, IUnitOfWork unitOfWork)
        {
            // Ayný bakým için zaten bildirim var mý kontrol et
            var existingNotification = await unitOfWork.VehicleNotifications.FirstOrDefaultAsync(n =>
                n.VehicleId == maintenance.VehicleId &&
                n.Type == NotificationType.MaintenanceDue &&
                n.AdditionalData != null &&
                n.AdditionalData.Contains(maintenance.Id.ToString()));

            if (existingNotification != null)
                return;

            var notification = new VehicleNotification(
                maintenance.VehicleId,
                NotificationType.MaintenanceDue,
                "Bakým Hatýrlatmasý",
                $"{maintenance.Vehicle.PlateNumber} plakalý araç için {daysUntil} gün sonra bakým gerekiyor. Bakým türü: {maintenance.Type}",
                DateTime.UtcNow,
                maintenance.Vehicle.AssignedUserId,
                2);

            notification.SetAdditionalData($"{{\"maintenanceId\": {maintenance.Id}}}");

            await unitOfWork.VehicleNotifications.AddAsync(notification);
        }

        private async Task CreateOverdueMaintenanceNotificationAsync(VehicleMaintenance maintenance, IUnitOfWork unitOfWork)
        {
            var notification = new VehicleNotification(
                maintenance.VehicleId,
                NotificationType.MaintenanceDue,
                "Geciken Bakým",
                $"{maintenance.Vehicle.PlateNumber} plakalý araç için bakým süresi geçti! Bakým türü: {maintenance.Type}",
                DateTime.UtcNow,
                maintenance.Vehicle.AssignedUserId,
                3);

            notification.SetAdditionalData($"{{\"maintenanceId\": {maintenance.Id}}}");

            await unitOfWork.VehicleNotifications.AddAsync(notification);
        }
    }
}