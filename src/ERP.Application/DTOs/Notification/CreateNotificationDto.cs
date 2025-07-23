using ERP.Core.Enums;

namespace ERP.Application.DTOs.Notification
{
    public class CreateNotificationDto
    {
        public int VehicleId { get; set; }
        public int? UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; } = DateTime.UtcNow;
        public int Priority { get; set; } = 1;
        public string? ActionUrl { get; set; }
        public string? AdditionalData { get; set; }
    }
}