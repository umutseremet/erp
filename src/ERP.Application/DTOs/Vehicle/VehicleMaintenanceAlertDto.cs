// VehicleMaintenanceAlertDto.cs
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleMaintenanceAlertDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public MaintenanceType MaintenanceType { get; set; }
        public DateTime DueDate { get; set; }
        public decimal? DueKilometer { get; set; }
        public decimal CurrentKilometer { get; set; }
        public int DaysOverdue { get; set; }
        public decimal KilometersOverdue { get; set; }
        public string AlertLevel { get; set; } = string.Empty; // Info, Warning, Critical
        public string Description { get; set; } = string.Empty;
        public decimal? EstimatedCost { get; set; }
        public string? RecommendedServiceProvider { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public bool IsOverdue => DaysOverdue > 0 || KilometersOverdue > 0;
        public string FormattedDueDate => DueDate.ToString("dd.MM.yyyy");
        public string FormattedEstimatedCost => EstimatedCost?.ToString("C") ?? "Belirtilmemiş";
    }
}