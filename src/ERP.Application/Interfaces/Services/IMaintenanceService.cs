// 1. IMaintenanceService.cs
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Dashboard;
using ERP.Application.DTOs.Maintenance;

namespace ERP.Application.Interfaces.Services
{
    public interface IMaintenanceService
    {
        Task<Result<MaintenanceDto>> CreateMaintenanceAsync(CreateMaintenanceDto dto);
        Task<Result<MaintenanceDto>> UpdateMaintenanceAsync(int id, UpdateMaintenanceDto dto);
        Task<Result<MaintenanceDto>> GetMaintenanceByIdAsync(int id);
        Task<PaginatedResult<MaintenanceDto>> GetMaintenancesByVehicleAsync(int vehicleId, PaginationDto pagination);
        Task<Result<MaintenanceDto>> CompleteMaintenanceAsync(int id, CompleteMaintenanceDto dto);
        Task<Result<MaintenanceDto>> ScheduleMaintenanceAsync(ScheduleMaintenanceDto dto);
        Task<Result<bool>> DeleteMaintenanceAsync(int id);
        Task<IEnumerable<UpcomingMaintenanceDto>> GetUpcomingMaintenanceAsync(int daysAhead = 30);
        Task<IEnumerable<MaintenanceDto>> GetOverdueMaintenanceAsync();
        Task<MaintenancePerformanceReportDto> GetMaintenancePerformanceReportAsync(DateTime startDate, DateTime endDate);
        Task<MaintenanceDashboardStats> GetMaintenanceDashboardStatsAsync();
        Task<IEnumerable<MaintenanceKPIDto>> GetMaintenanceKPIsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ServiceProviderPerformanceDto>> GetServiceProviderPerformanceAsync(DateTime startDate, DateTime endDate);
        Task<MaintenanceEfficiencyDto> GetMaintenanceEfficiencyAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<EfficiencyImprovementDto>> GetEfficiencyImprovementSuggestionsAsync();
    }
}
