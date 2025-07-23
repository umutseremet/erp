namespace ERP.Application.DTOs.Maintenance
{
    public class UpcomingMaintenanceDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public string MaintenanceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public decimal VehicleKm { get; set; }
        public string? ServiceProvider { get; set; }
        public decimal? EstimatedCost { get; set; }
        public int DaysUntilDue { get; set; }
        public string Priority { get; set; } = string.Empty; // High, Medium, Low
        public string Status { get; set; } = string.Empty; // Scheduled, Pending, Overdue
        public string? Notes { get; set; }
        public bool CanBePostponed { get; set; }
        public DateTime? MaxPostponeDate { get; set; }
    }
}