namespace ERP.Application.DTOs.Notification
{
    public class UpdateNotificationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public int Priority { get; set; }
        public string? ActionUrl { get; set; }
        public string? AdditionalData { get; set; }
    }
}