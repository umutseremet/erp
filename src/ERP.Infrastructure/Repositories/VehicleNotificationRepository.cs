using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class VehicleNotificationRepository : BaseRepository<VehicleNotification>, IVehicleNotificationRepository
    {
        public VehicleNotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleNotification>> GetByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => vn.VehicleId == vehicleId)
                .OrderByDescending(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleNotification>> GetByUserAsync(int userId)
        {
            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => vn.UserId == userId)
                .OrderByDescending(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleNotification>> GetByTypeAsync(NotificationType type)
        {
            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => vn.Type == type)
                .OrderByDescending(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleNotification>> GetUnreadByUserAsync(int userId)
        {
            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => vn.UserId == userId && !vn.IsRead)
                .OrderByDescending(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleNotification>> GetPendingNotificationsAsync()
        {
            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => !vn.IsSent && vn.ScheduledDate <= DateTime.UtcNow)
                .OrderBy(vn => vn.Priority)
                .ThenBy(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleNotification>> GetDueNotificationsAsync()
        {
            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => !vn.IsSent && vn.ScheduledDate <= DateTime.UtcNow)
                .OrderBy(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleNotification>> GetOverdueNotificationsAsync()
        {
            var oneDayAgo = DateTime.UtcNow.AddDays(-1);

            return await _dbSet
                .Include(vn => vn.Vehicle)
                .Include(vn => vn.User)
                .Where(vn => !vn.IsSent && vn.ScheduledDate <= oneDayAgo)
                .OrderBy(vn => vn.ScheduledDate)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountByUserAsync(int userId)
        {
            return await _dbSet
                .CountAsync(vn => vn.UserId == userId && !vn.IsRead);
        }

        public async Task MarkAllAsReadByUserAsync(int userId)
        {
            var notifications = await _dbSet
                .Where(vn => vn.UserId == userId && !vn.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.MarkAsRead();
            }
        }
    }
}