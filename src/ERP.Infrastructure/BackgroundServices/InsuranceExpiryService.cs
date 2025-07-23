using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ERP.Core.Interfaces;
using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Infrastructure.BackgroundServices
{
    public class InsuranceExpiryService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InsuranceExpiryService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromHours(12);

        public InsuranceExpiryService(IServiceProvider serviceProvider, ILogger<InsuranceExpiryService> logger)
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
                    await CheckInsuranceExpiriesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking insurance expiries");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }

        private async Task CheckInsuranceExpiriesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var expiringPolicies = await unitOfWork.InsurancePolicies.GetExpiringPoliciesAsync(30);

            foreach (var policy in expiringPolicies)
            {
                try
                {
                    var daysUntilExpiry = (policy.EndDate - DateTime.UtcNow).Days;

                    if (daysUntilExpiry <= 30 && daysUntilExpiry > 0)
                    {
                        await CreateInsuranceExpiryNotificationAsync(policy, daysUntilExpiry, unitOfWork);
                    }
                    else if (daysUntilExpiry <= 0)
                    {
                        await CreateExpiredInsuranceNotificationAsync(policy, unitOfWork);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to create insurance expiry notification for policy {policy.Id}");
                }
            }

            await unitOfWork.SaveChangesAsync();
        }

        private async Task CreateInsuranceExpiryNotificationAsync(InsurancePolicy policy, int daysUntil, IUnitOfWork unitOfWork)
        {
            // Ayný poliçe için zaten bildirim var mý kontrol et
            var existingNotification = await unitOfWork.VehicleNotifications.FirstOrDefaultAsync(n =>
                n.VehicleId == policy.VehicleId &&
                n.Type == NotificationType.InsuranceExpiring &&
                n.AdditionalData != null &&
                n.AdditionalData.Contains(policy.Id.ToString()));

            if (existingNotification != null)
                return;

            var priority = daysUntil <= 7 ? 3 : 2;

            var notification = new VehicleNotification(
                policy.VehicleId,
                NotificationType.InsuranceExpiring,
                "Sigorta Süresi Dolacak",
                $"{policy.Vehicle.PlateNumber} plakalý araç için {policy.PolicyType} sigortasý {daysUntil} gün sonra sona erecek. Poliçe No: {policy.PolicyNumber}",
                DateTime.UtcNow,
                policy.Vehicle.AssignedUserId,
                priority);

            notification.SetAdditionalData($"{{\"policyId\": {policy.Id}}}");

            await unitOfWork.VehicleNotifications.AddAsync(notification);
        }

        private async Task CreateExpiredInsuranceNotificationAsync(InsurancePolicy policy, IUnitOfWork unitOfWork)
        {
            var notification = new VehicleNotification(
                policy.VehicleId,
                NotificationType.InsuranceExpiring,
                "Sigorta Süresi Doldu",
                $"{policy.Vehicle.PlateNumber} plakalý araç için {policy.PolicyType} sigortasýnýn süresi doldu! Poliçe No: {policy.PolicyNumber}",
                DateTime.UtcNow,
                policy.Vehicle.AssignedUserId,
                4);

            notification.SetAdditionalData($"{{\"policyId\": {policy.Id}}}");

            await unitOfWork.VehicleNotifications.AddAsync(notification);
        }
    }
}