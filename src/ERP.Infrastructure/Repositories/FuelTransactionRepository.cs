using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class FuelTransactionRepository : BaseRepository<FuelTransaction>, IFuelTransactionRepository
    {
        public FuelTransactionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FuelTransaction>> GetByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(ft => ft.Vehicle)
                .Include(ft => ft.FuelCard)
                .Where(ft => ft.VehicleId == vehicleId)
                .OrderByDescending(ft => ft.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FuelTransaction>> GetByFuelCardAsync(int fuelCardId)
        {
            return await _dbSet
                .Include(ft => ft.Vehicle)
                .Include(ft => ft.FuelCard)
                .Where(ft => ft.FuelCardId == fuelCardId)
                .OrderByDescending(ft => ft.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FuelTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(ft => ft.Vehicle)
                .Include(ft => ft.FuelCard)
                .Where(ft => ft.TransactionDate >= startDate && ft.TransactionDate <= endDate)
                .OrderByDescending(ft => ft.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<FuelTransaction>> GetByVehicleAndDateRangeAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(ft => ft.Vehicle)
                .Include(ft => ft.FuelCard)
                .Where(ft => ft.VehicleId == vehicleId &&
                            ft.TransactionDate >= startDate &&
                            ft.TransactionDate <= endDate)
                .OrderByDescending(ft => ft.TransactionDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalFuelCostByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(ft => ft.VehicleId == vehicleId);

            if (startDate.HasValue)
                query = query.Where(ft => ft.TransactionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ft => ft.TransactionDate <= endDate.Value);

            return await query.SumAsync(ft => ft.TotalAmount);
        }

        public async Task<decimal> GetTotalFuelQuantityByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _dbSet.Where(ft => ft.VehicleId == vehicleId);

            if (startDate.HasValue)
                query = query.Where(ft => ft.TransactionDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(ft => ft.TransactionDate <= endDate.Value);

            return await query.SumAsync(ft => ft.Quantity);
        }

        public async Task<FuelTransaction?> GetLastTransactionByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(ft => ft.Vehicle)
                .Include(ft => ft.FuelCard)
                .Where(ft => ft.VehicleId == vehicleId)
                .OrderByDescending(ft => ft.TransactionDate)
                .FirstOrDefaultAsync();
        }
    }
}