using ERP.Application.DTOs.Common;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Notification
{
    public class NotificationSummaryDto
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
        public int UnreadCount { get; set; }
        public DateTime? LatestDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}