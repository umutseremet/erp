using ERP.Application.Common.Models;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Notification;

namespace ERP.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<Result<NotificationDto>> CreateNotificationAsync(CreateNotificationDto dto);
        Task<Result<NotificationDto>> GetNotificationByIdAsync(int id);
        Task<PaginatedResult<NotificationDto>> GetNotificationsByUserAsync(int userId, PaginationDto pagination);
        Task<PaginatedResult<NotificationDto>> GetNotificationsByVehicleAsync(int vehicleId, PaginationDto pagination);
        Task<Result<bool>> MarkAsReadAsync(int notificationId);
        Task<Result<bool>> MarkAllAsReadByUserAsync(int userId);
        Task<Result<bool>> DeleteNotificationAsync(int id);
        Task<IEnumerable<NotificationDto>> GetUnreadNotificationsByUserAsync(int userId);
        Task<int> GetUnreadCountByUserAsync(int userId);
        Task<Result<bool>> SendNotificationAsync(int notificationId);
        Task<Result<bool>> ScheduleInsuranceExpiryNotificationAsync(int vehicleId, DateTime expiryDate);
        Task<Result<bool>> ScheduleMaintenanceReminderAsync(int maintenanceId, DateTime reminderDate);
        Task<Result<bool>> CreateBulkNotificationsAsync(IEnumerable<CreateNotificationDto> notifications);
        Task<IEnumerable<NotificationDto>> GetPendingNotificationsAsync();
        Task<IEnumerable<NotificationDto>> GetOverdueNotificationsAsync();
    }
}