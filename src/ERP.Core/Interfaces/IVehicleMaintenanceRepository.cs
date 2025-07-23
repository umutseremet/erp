using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Core.Interfaces
{
    public interface IVehicleMaintenanceRepository : IRepository<VehicleMaintenance>
    {
        Task<IEnumerable<VehicleMaintenance>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<VehicleMaintenance>> GetByTypeAsync(MaintenanceType type);
        Task<IEnumerable<VehicleMaintenance>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<VehicleMaintenance>> GetUpcomingMaintenanceAsync();
        Task<IEnumerable<VehicleMaintenance>> GetOverdueMaintenanceAsync();
        Task<VehicleMaintenance?> GetLastMaintenanceByVehicleAsync(int vehicleId);
        Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
    }
}