using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;

namespace ERP.Core.Entities
{
    public class VehicleNotification : BaseEntity
    {
        public int VehicleId { get; private set; }
        public int? UserId { get; private set; }
        
        public NotificationType Type { get; private set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string Message { get; private set; } = string.Empty;
        
        public DateTime ScheduledDate { get; private set; }
        public DateTime? SentDate { get; private set; }
        
        public bool IsRead { get; private set; } = false;
        public bool IsSent { get; private set; } = false;
        
        public int Priority { get; private set; } = 1; // 1=Low, 2=Medium, 3=High, 4=Critical
        
        [StringLength(500)]
        public string? ActionUrl { get; private set; }
        
        public string? AdditionalData { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual User? User { get; set; }

        protected VehicleNotification() { }

        public VehicleNotification(int vehicleId, NotificationType type, string title, string message, 
                                 DateTime scheduledDate, int? userId = null, int priority = 1)
        {
            VehicleId = vehicleId;
            UserId = userId;
            Type = type;
            SetTitle(title);
            SetMessage(message);
            ScheduledDate = scheduledDate;
            SetPriority(priority);
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Bildirim başlığı boş olamaz");

            Title = title.Trim();
            UpdateTimestamp();
        }

        public void SetMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Bildirim mesajı boş olamaz");

            Message = message.Trim();
            UpdateTimestamp();
        }

        public void SetPriority(int priority)
        {
            if (priority < 1 || priority > 4)
                throw new ArgumentException("Öncelik 1-4 arasında olmalıdır");

            Priority = priority;
            UpdateTimestamp();
        }

        public void SetActionUrl(string? actionUrl)
        {
            ActionUrl = actionUrl?.Trim();
            UpdateTimestamp();
        }

        public void SetAdditionalData(string? additionalData)
        {
            AdditionalData = additionalData?.Trim();
            UpdateTimestamp();
        }

        public void MarkAsSent()
        {
            IsSent = true;
            SentDate = DateTime.UtcNow;
            UpdateTimestamp();
        }

        public void MarkAsRead()
        {
            IsRead = true;
            UpdateTimestamp();
        }

        public void Reschedule(DateTime newScheduledDate)
        {
            if (IsSent)
                throw new InvalidOperationException("Gönderilmiş bildirim yeniden planlanamaz");

            ScheduledDate = newScheduledDate;
            UpdateTimestamp();
        }

        public bool IsDue => DateTime.UtcNow >= ScheduledDate;
        public bool IsOverdue => !IsSent && DateTime.UtcNow > ScheduledDate.AddHours(24);
        
        public string GetPriorityText()
        {
            return Priority switch
            {
                1 => "Düşük",
                2 => "Orta",
                3 => "Yüksek",
                4 => "Kritik",
                _ => "Bilinmiyor"
            };
        }
    }
}