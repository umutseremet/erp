using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleReportRequestDto
    {
        public VehicleStatus? Status { get; set; }
        public VehicleType? Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DepartmentId { get; set; }
        public int? UserId { get; set; }
        public ReportFormat Format { get; set; } = ReportFormat.Excel;
        public bool IncludeMaintenanceHistory { get; set; } = false;
        public bool IncludeFuelHistory { get; set; } = false;
        public bool IncludeInsuranceInfo { get; set; } = false;
        public bool IncludeLocationHistory { get; set; } = false; 
    }

    public enum ReportFormat
    {
        Excel = 1,
        Pdf = 2,
        Csv = 3
    }
}