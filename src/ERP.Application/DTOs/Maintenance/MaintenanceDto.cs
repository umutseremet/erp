using ERP.Application.DTOs.Common;

namespace ERP.Application.DTOs.Maintenance
{
    public class MaintenanceDto : BaseDto
    {
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string MaintenanceType { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal VehicleKm { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ServiceProvider { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public string? Notes { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysOverdue { get; set; }
    }
}