using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Core.Interfaces
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
        Task<Vehicle?> GetByPlateNumberAsync(string plateNumber);
        Task<Vehicle?> GetByVinNumberAsync(string vinNumber);
        Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status);
        Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetAssignedVehiclesAsync();
        Task<IEnumerable<Vehicle>> GetVehiclesByUserAsync(int userId);
        Task<IEnumerable<Vehicle>> GetVehiclesNeedingMaintenanceAsync();
        Task<IEnumerable<Vehicle>> GetVehiclesNeedingInspectionAsync();
        Task<bool> IsPlateNumberExistsAsync(string plateNumber, int? excludeId = null);
        Task<bool> IsVinNumberExistsAsync(string vinNumber, int? excludeId = null);
    }
}