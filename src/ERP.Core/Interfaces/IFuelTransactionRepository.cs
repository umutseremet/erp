using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Core.Interfaces
{
    public interface IFuelTransactionRepository : IRepository<FuelTransaction>
    {
        Task<IEnumerable<FuelTransaction>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<FuelTransaction>> GetByFuelCardAsync(int fuelCardId);
        Task<IEnumerable<FuelTransaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<FuelTransaction>> GetByVehicleAndDateRangeAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalFuelCostByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalFuelQuantityByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
        Task<FuelTransaction?> GetLastTransactionByVehicleAsync(int vehicleId);
    }
}