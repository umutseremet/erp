using ERP.Application.Common.Models;
using ERP.Application.DTOs.Dashboard;
using ERP.Application.DTOs.Reports;
using ERP.Application.DTOs.Vehicle;

namespace ERP.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<Result<byte[]>> GenerateVehicleReportAsync(VehicleReportRequestDto request);
        Task<Result<byte[]>> GenerateMaintenanceReportAsync(MaintenanceReportRequestDto request);
        Task<Result<byte[]>> GenerateFuelReportAsync(FuelReportRequestDto request);
        Task<Result<byte[]>> GenerateInsuranceReportAsync(InsuranceReportRequestDto request);
        Task<Result<byte[]>> GenerateUserReportAsync(UserReportRequestDto request);
        Task<Result<byte[]>> GenerateDepartmentReportAsync(DepartmentReportRequestDto request);
        Task<Result<DashboardDataDto>> GetDashboardDataAsync(int? userId = null);
        Task<Result<byte[]>> GenerateExecutiveSummaryAsync(ExecutiveSummaryRequestDto request);
        Task<Result<byte[]>> GenerateCustomReportAsync(CustomReportRequestDto request);
        Task<Result<byte[]>> GenerateFleetPerformanceReportAsync(FleetPerformanceRequestDto request);
        Task<Result<byte[]>> GenerateCostAnalysisReportAsync(CostAnalysisRequestDto request);
        Task<Result<string[]>> GetAvailableReportTemplatesAsync();
        Task<Result<byte[]>> CreateReportTemplateAsync(CreateReportTemplateDto request);
        Task<Result<bool>> DeleteReportTemplateAsync(int templateId);
        Task<Result<byte[]>> GetKPIDashboardAsync(DateTime startDate, DateTime endDate);
        Task<Result<byte[]>> ScheduleReportAsync(ScheduledReportDto request);
        Task<Result<IEnumerable<ScheduledReportDto>>> GetScheduledReportsAsync();
    }
}