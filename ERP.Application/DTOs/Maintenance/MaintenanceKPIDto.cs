namespace ERP.Application.DTOs.Maintenance
{
    public class MaintenanceKPIDto
    {
        public string KPIName { get; set; } = string.Empty;
        public decimal CurrentValue { get; set; }
        public decimal PreviousValue { get; set; }
        public decimal ChangePercentage { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Trend { get; set; } = string.Empty; // Up, Down, Stable
        public string Status { get; set; } = string.Empty; // Good, Warning, Critical
        public string Description { get; set; } = string.Empty;
        public DateTime CalculatedAt { get; set; }
    }
}