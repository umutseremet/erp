using ERP.Core.Enums;

namespace ERP.Application.DTOs.Notification
{
    public class NotificationListDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public DateTime? SentDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedAt { get; set; }

        public string TypeText => Type switch
        {
            NotificationType.MaintenanceDue => "Bakım Zamanı",
            NotificationType.InspectionDue => "Muayene Zamanı",
            NotificationType.InsuranceExpiring => "Sigorta Süresi Bitiyor",
            NotificationType.LicenseExpiring => "Ruhsat Süresi Bitiyor",
            NotificationType.FuelCardExpiring => "Yakıt Kartı Süresi Bitiyor",
            NotificationType.VehicleAssigned => "Araç Atandı",
            NotificationType.VehicleReturned => "Araç Teslim Alındı",
            NotificationType.AccidentReported => "Kaza Bildirildi",
            NotificationType.ServiceCompleted => "Servis Tamamlandı",
            NotificationType.OverdueReturn => "Geciken İade",
            _ => Type.ToString()
        };

        public string PriorityText => Priority switch
        {
            1 => "Düşük",
            2 => "Orta",
            3 => "Yüksek",
            4 => "Kritik",
            _ => "Bilinmiyor"
        };

        public string StatusText => IsSent ? (IsRead ? "Okundu" : "Gönderildi") : "Bekliyor";
        public string FormattedScheduledDate => ScheduledDate.ToString("dd.MM.yyyy HH:mm");
        public string FormattedSentDate => SentDate?.ToString("dd.MM.yyyy HH:mm") ?? "-";
    }
}