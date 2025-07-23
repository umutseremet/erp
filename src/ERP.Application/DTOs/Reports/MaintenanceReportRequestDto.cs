using ERP.Application.DTOs.Vehicle;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Reports
{
    public class MaintenanceReportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VehicleId { get; set; }
        public MaintenanceType? MaintenanceType { get; set; }
        public string? ServiceProvider { get; set; }
        public bool? IsCompleted { get; set; }
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
        public bool IncludeCostAnalysis { get; set; } = true;
        public bool IncludePerformanceMetrics { get; set; } = true;
    }

    public class FuelReportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VehicleId { get; set; }
        public FuelType? FuelType { get; set; }
        public int? FuelCardId { get; set; }
        public string? StationName { get; set; }
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
        public bool IncludeEfficiencyAnalysis { get; set; } = true;
        public bool IncludeCostTrends { get; set; } = true;
    }

    public class InsuranceReportRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VehicleId { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? PolicyType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsExpiring { get; set; }
        public int ExpiringDays { get; set; } = 30;
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
    }

    public class UserReportRequestDto
    {
        public int? DepartmentId { get; set; }
        public UserStatus? UserStatus { get; set; }
        //public assi? HasAssignedVehicles { get; set; }
        public DateTime? LastLoginAfter { get; set; }
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
        public bool IncludeVehicleAssignments { get; set; } = true;
        public bool IncludeDepartmentInfo { get; set; } = true;
    }

    public class DepartmentReportRequestDto
    {
        public int? ParentDepartmentId { get; set; }
        public bool? IsActive { get; set; }
        public bool IncludeUserCount { get; set; } = true;
        public bool IncludeVehicleCount { get; set; } = true;
        public bool IncludeHierarchy { get; set; } = true;
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
    }

    public class ExecutiveSummaryRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IncludeFinancialSummary { get; set; } = true;
        public bool IncludeOperationalMetrics { get; set; } = true;
        public bool IncludeComplianceStatus { get; set; } = true;
        public bool IncludeRecommendations { get; set; } = true;
        public ReportFormat Format { get; set; } = ReportFormat.Pdf;
    }

    public class CustomReportRequestDto
    {
        public string ReportName { get; set; } = string.Empty;
        public string? TemplateId { get; set; }
        public Dictionary<string, object> Parameters { get; set; } = new();
        public List<string> IncludedTables { get; set; } = new();
        public List<string> IncludedColumns { get; set; } = new();
        public Dictionary<string, object> Filters { get; set; } = new();
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
    }

    public class FleetPerformanceRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? DepartmentId { get; set; }
        public string? VehicleType { get; set; }
        public bool IncludeFuelEfficiency { get; set; } = true;
        public bool IncludeMaintenanceCosts { get; set; } = true;
        public bool IncludeUtilizationRate { get; set; } = true;
        public bool IncludeSafetyMetrics { get; set; } = true;
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
    }

    public class CostAnalysisRequestDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> CostCategories { get; set; } = new();
        public int? VehicleId { get; set; }
        public int? DepartmentId { get; set; }
        public bool IncludeBudgetComparison { get; set; } = true;
        public bool IncludeTrends { get; set; } = true;
        public bool IncludeProjections { get; set; } = false;
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
    }

    public class CreateReportTemplateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string TemplateContent { get; set; } = string.Empty;
        public Dictionary<string, object> DefaultParameters { get; set; } = new();
        public List<string> RequiredPermissions { get; set; } = new();
        public bool IsActive { get; set; } = true;
    }

    public class ScheduledReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
        public string Schedule { get; set; } = string.Empty; // Cron expression
        public bool IsActive { get; set; } = true;
        public DateTime? LastRun { get; set; }
        public DateTime? NextRun { get; set; }
        public List<string> Recipients { get; set; } = new();
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
        public string? DeliveryMethod { get; set; } = "Email";
    }
}