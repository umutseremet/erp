using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace ERP.Infrastructure.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Vehicle?> GetByPlateNumberAsync(string plateNumber)
        {
            return await _dbSet
                .Include(v => v.AssignedUser)
                .FirstOrDefaultAsync(v => v.PlateNumber == plateNumber);
        }

        public async Task<Vehicle?> GetByVinNumberAsync(string vinNumber)
        {
            return await _dbSet
                .Include(v => v.AssignedUser)
                .FirstOrDefaultAsync(v => v.VinNumber == vinNumber);
        }

        public async Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status)
        {
            return await _dbSet
                .Include(v => v.AssignedUser)
                .Where(v => v.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync()
        {
            return await GetByStatusAsync(VehicleStatus.Available);
        }

        public async Task<IEnumerable<Vehicle>> GetAssignedVehiclesAsync()
        {
            return await GetByStatusAsync(VehicleStatus.Assigned);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByUserAsync(int userId)
        {
            return await _dbSet
                .Include(v => v.AssignedUser)
                .Where(v => v.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesNeedingMaintenanceAsync()
        {
            var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);

            return await _dbSet
                .Include(v => v.Maintenances)
                .Where(v => v.Maintenances.Any(m =>
                    !m.IsCompleted &&
                    m.ScheduledDate <= thirtyDaysFromNow))
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesNeedingInspectionAsync()
        {
            var thirtyDaysFromNow = DateTime.UtcNow.AddDays(30);

            return await _dbSet
                .Include(v => v.Inspections)
                .Where(v => v.Inspections.Any(i =>
                    i.ExpiryDate <= thirtyDaysFromNow))
                .ToListAsync();
        }

        public async Task<bool> IsPlateNumberExistsAsync(string plateNumber, int? excludeId = null)
        {
            var query = _dbSet.Where(v => v.PlateNumber == plateNumber);

            if (excludeId.HasValue)
            {
                query = query.Where(v => v.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsVinNumberExistsAsync(string vinNumber, int? excludeId = null)
        {
            var query = _dbSet.Where(v => v.VinNumber == vinNumber);

            if (excludeId.HasValue)
            {
                query = query.Where(v => v.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}