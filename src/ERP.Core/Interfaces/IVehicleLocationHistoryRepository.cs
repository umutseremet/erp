using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IVehicleLocationHistoryRepository : IRepository<VehicleLocationHistory>
    {
        Task<IEnumerable<VehicleLocationHistory>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<VehicleLocationHistory>> GetByVehicleAndDateRangeAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task<VehicleLocationHistory?> GetLastLocationAsync(int vehicleId);
        Task<IEnumerable<VehicleLocationHistory>> GetLocationsByAreaAsync(decimal centerLat, decimal centerLon, decimal radiusKm);
        Task<decimal> GetTotalDistanceAsync(int vehicleId, DateTime startDate, DateTime endDate);
        Task DeleteOldLocationsAsync(int vehicleId, DateTime olderThan);
    }
}