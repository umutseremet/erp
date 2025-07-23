using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class VehicleMaintenanceRepository : BaseRepository<VehicleMaintenance>, IVehicleMaintenanceRepository
    {
        public VehicleMaintenanceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleMaintenance>> GetByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(vm => vm.Vehicle)
                .Where(vm => vm.VehicleId == vehicleId)
                .OrderByDescending(vm => vm.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleMaintenance>> GetByTypeAsync(MaintenanceType type)
        {
            return await _dbSet
                .Include(vm => vm.Vehicle)
                .Where(vm => vm.Type == type)
                .OrderByDescending(vm => vm.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleMaintenance>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(vm => vm.Vehicle)
                .Where(vm => vm.ScheduledDate >= startDate && vm.ScheduledDate <= endDate)
                .OrderBy(vm => vm.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleMaintenance>> GetUpcomingMaintenanceAsync()
        {
            var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);

            return await _dbSet
                .Include(vm => vm.Vehicle)
                .Where(vm => !vm.IsCompleted &&
                            vm.ScheduledDate <= thirtyDaysFromNow)
                .OrderBy(vm => vm.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleMaintenance>> GetOverdueMaintenanceAsync()
        {
            return await _dbSet
                .Include(vm => vm.Vehicle)
                .Where(vm => !vm.IsCompleted &&
                            vm.ScheduledDate < DateTime.UtcNow)
                .OrderBy(vm => vm.ScheduledDate)
                .ToListAsync();
        }

        public async Task<VehicleMaintenance?> GetLastMaintenanceByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(vm => vm.Vehicle)
                .Where(vm => vm.VehicleId == vehicleId && vm.IsCompleted)
                .OrderByDescending(vm => vm.CompletedDate)
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(vm => vm.VehicleId == vehicleId && vm.Cost.HasValue);

            if (startDate.HasValue)
                query = query.Where(vm => vm.ScheduledDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(vm => vm.ScheduledDate <= endDate.Value);

            return await query.SumAsync(vm => vm.Cost ?? 0);
        }
    }
}