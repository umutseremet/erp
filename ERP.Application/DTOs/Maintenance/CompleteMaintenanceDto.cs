namespace ERP.Application.DTOs.Maintenance
{
    public class CompleteMaintenanceDto
    {
        public DateTime CompletedDate { get; set; }
        public decimal? ActualCost { get; set; }
        public string? CompletionNotes { get; set; }
        public string? ServiceProvider { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public List<string>? CompletedTasks { get; set; }
        public List<string>? ReplacedParts { get; set; }
        public int? QualityRating { get; set; } // 1-5
        public bool CreateReminder { get; set; } = true;
    }
}