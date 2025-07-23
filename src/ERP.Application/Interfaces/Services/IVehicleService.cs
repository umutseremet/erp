using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Core.Enums;

namespace ERP.Application.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<Result<VehicleDto>> GetVehicleByIdAsync(int id);
        Task<Result<VehicleDto>> GetVehicleByPlateNumberAsync(string plateNumber);
        Task<Result<VehicleDto>> GetVehicleByVinNumberAsync(string vinNumber);
        Task<PaginatedResult<VehicleListDto>> GetVehiclesPagedAsync(VehicleFilterDto filter, int pageNumber, int pageSize);
        Task<Result<IEnumerable<VehicleDto>>> GetAllVehiclesAsync();
        Task<Result<IEnumerable<VehicleDto>>> GetAvailableVehiclesAsync();
        Task<Result<IEnumerable<VehicleDto>>> GetAssignedVehiclesAsync();
        Task<Result<IEnumerable<VehicleDto>>> GetVehiclesByUserAsync(int userId);
        Task<Result<IEnumerable<VehicleDto>>> GetVehiclesByStatusAsync(VehicleStatus status);
        Task<Result<IEnumerable<VehicleDto>>> GetVehiclesNeedingMaintenanceAsync();
        Task<Result<IEnumerable<VehicleDto>>> GetVehiclesNeedingInspectionAsync();
        Task<Result<VehicleDto>> CreateVehicleAsync(CreateVehicleDto dto);
        Task<Result<VehicleDto>> UpdateVehicleAsync(int id, UpdateVehicleDto dto);
        Task<Result<bool>> DeleteVehicleAsync(int id);
        Task<Result<bool>> AssignVehicleToUserAsync(int vehicleId, int userId);
        Task<Result<bool>> ReturnVehicleFromUserAsync(int vehicleId, string? notes = null);
        Task<Result<bool>> UpdateVehicleStatusAsync(int id, VehicleStatus status, string? reason = null);
        Task<Result<bool>> UpdateVehicleKilometerAsync(int id, decimal km, string? notes = null);
        Task<Result<IEnumerable<VehicleDto>>> GetVehicleAssignmentHistoryAsync(int vehicleId);
        Task<Result<IEnumerable<VehicleDto>>> GetVehicleLocationHistoryAsync(int vehicleId, DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<bool>> SendVehicleToMaintenanceAsync(int vehicleId, string reason, DateTime? scheduledDate = null);
        Task<Result<bool>> ReturnVehicleFromMaintenanceAsync(int vehicleId, string? notes = null);
        Task<Result<VehicleStatisticsDto>> GetVehicleStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<IEnumerable<VehicleDto>>> GetFleetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<IEnumerable<VehicleDto>>> GetFleetPerformanceReportAsync(VehicleReportFilterDto filter);
        Task<Result<IEnumerable<VehicleDto>>> GetVehicleUtilizationReportAsync(VehicleReportFilterDto filter);
        Task<Result<bool>> IsPlateNumberUniqueAsync(string plateNumber, int? excludeVehicleId = null);
        Task<Result<bool>> IsVinNumberUniqueAsync(string vinNumber, int? excludeVehicleId = null);
        Task<Result<IEnumerable<VehicleDto>>> ExportVehiclesAsync(VehicleExportFilterDto filter);
        Task<Result<bool>> ImportVehiclesAsync(byte[] data, VehicleImportFormat format);
        Task<Result<IEnumerable<VehicleDto>>> CreateMultipleVehiclesAsync(IEnumerable<CreateVehicleDto> vehicles);
        Task<Result<IEnumerable<VehicleDto>>> UpdateMultipleVehiclesAsync(IEnumerable<UpdateVehicleBulkDto> vehicles);
        Task<Result<IEnumerable<int>>> DeleteMultipleVehiclesAsync(IEnumerable<int> vehicleIds);
        Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> vehicleIds, VehicleStatus status);
        Task<Result<IEnumerable<VehicleDto>>> GetVehicleAlertsAsync();
        Task<Result<IEnumerable<VehicleDto>>> GetCriticalVehicleAlertsAsync();
    }
}