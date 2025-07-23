using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ERP.Core.Interfaces;
using ERP.Infrastructure.ExternalServices.Email;
using ERP.Infrastructure.ExternalServices.SMS;

namespace ERP.Infrastructure.BackgroundServices
{
    public class NotificationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(5);

        public NotificationService(IServiceProvider serviceProvider, ILogger<NotificationService> logger)
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
                    await ProcessNotificationsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing notifications");
                }

                await Task.Delay(_period, stoppingToken);
            }
        }

        private async Task ProcessNotificationsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

            var pendingNotifications = await unitOfWork.VehicleNotifications.GetPendingNotificationsAsync();

            foreach (var notification in pendingNotifications)
            {
                try
                {
                    if (notification.User != null)
                    {
                        // Email gönder
                        await emailService.SendEmailAsync(
                            notification.User.Email,
                            notification.Title,
                            notification.Message,
                            true);

                        // SMS gönder (yüksek öncelikli bildirimler için)
                        if (notification.Priority >= 3 && !string.IsNullOrEmpty(notification.User.PhoneNumber))
                        {
                            await smsService.SendSmsAsync(
                                notification.User.PhoneNumber,
                                $"{notification.Title}: {notification.Message}");
                        }
                    }

                    notification.MarkAsSent();
                    await unitOfWork.SaveChangesAsync();

                    _logger.LogInformation($"Notification {notification.Id} sent successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send notification {notification.Id}");
                }
            }
        }
    }
}