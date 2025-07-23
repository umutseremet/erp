namespace ERP.Application.DTOs.Redmine
{
    public class RedmineSyncLogDto
    {
        public int Id { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = string.Empty; // Success, Failed, InProgress
        public int RecordsProcessed { get; set; }
        public int RecordsCreated { get; set; }
        public int RecordsUpdated { get; set; }
        public int RecordsFailed { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Details { get; set; }
        public TimeSpan? Duration => EndTime.HasValue ? EndTime.Value - StartTime : null;
    }
}