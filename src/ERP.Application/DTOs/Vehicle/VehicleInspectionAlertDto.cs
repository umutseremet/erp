namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleInspectionAlertDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public DateTime InspectionDueDate { get; set; }
        public DateTime? LastInspectionDate { get; set; }
        public string InspectionType { get; set; } = string.Empty;
        public int DaysUntilDue { get; set; }
        public int DaysOverdue { get; set; }
        public string AlertLevel { get; set; } = string.Empty; // Info, Warning, Critical
        public string Status { get; set; } = string.Empty; // Current, Expiring, Expired
        public string? InspectionCenter { get; set; }
        public decimal? EstimatedCost { get; set; }
        public bool IsLegalRequirement { get; set; } = true;
        public string? CertificateNumber { get; set; }
        public bool IsOverdue => DaysOverdue > 0;
        public bool IsExpiringSoon => DaysUntilDue <= 30 && DaysUntilDue > 0;
        public string FormattedDueDate => InspectionDueDate.ToString("dd.MM.yyyy");
        public string FormattedLastInspection => LastInspectionDate?.ToString("dd.MM.yyyy") ?? "Kayıt yok";
    }
}