using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Core.Interfaces
{
    public interface IVehicleNotificationRepository : IRepository<VehicleNotification>
    {
        Task<IEnumerable<VehicleNotification>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<VehicleNotification>> GetByUserAsync(int userId);
        Task<IEnumerable<VehicleNotification>> GetByTypeAsync(NotificationType type);
        Task<IEnumerable<VehicleNotification>> GetUnreadByUserAsync(int userId);
        Task<IEnumerable<VehicleNotification>> GetPendingNotificationsAsync();
        Task<IEnumerable<VehicleNotification>> GetDueNotificationsAsync();
        Task<IEnumerable<VehicleNotification>> GetOverdueNotificationsAsync();
        Task<int> GetUnreadCountByUserAsync(int userId);
        Task MarkAllAsReadByUserAsync(int userId);
    }
}