using ERP.Application.DTOs.Common;
using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Notification
{
    public class NotificationDto : BaseDto
    {
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public int Priority { get; set; }
        public string PriorityText { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
        public string? AdditionalData { get; set; }
        public bool IsDue { get; set; }
        public bool IsOverdue { get; set; }
    }
}