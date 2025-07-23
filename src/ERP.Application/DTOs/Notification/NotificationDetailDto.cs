using ERP.Core.Enums;

namespace ERP.Application.DTOs.Notification
{
    public class NotificationDetailDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public int Priority { get; set; }
        public string? ActionUrl { get; set; }
        public string? AdditionalData { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDue => DateTime.UtcNow >= ScheduledDate;
        public bool IsOverdue => !IsSent && DateTime.UtcNow > ScheduledDate.AddHours(24);
        public string StatusText => IsSent ? (IsRead ? "Okundu" : "Gönderildi") : "Bekliyor";
        public string PriorityText => Priority switch
        {
            1 => "Düşük",
            2 => "Orta",
            3 => "Yüksek",
            4 => "Kritik",
            _ => "Bilinmiyor"
        };
    }
}