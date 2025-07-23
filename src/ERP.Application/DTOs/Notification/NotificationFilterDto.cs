using ERP.Core.Enums;

namespace ERP.Application.DTOs.Notification
{
    public class NotificationFilterDto
    {
        public int? VehicleId { get; set; }
        public int? UserId { get; set; }
        public NotificationType? Type { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsSent { get; set; }
        public int? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}