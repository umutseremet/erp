using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;

namespace ERP.Core.Entities
{
    public class SystemLog : BaseEntity
    {
        public LogLevel Level { get; private set; }
        
        [Required]
        [StringLength(500)]
        public string Message { get; private set; } = string.Empty;
        
        [StringLength(100)]
        public string? Source { get; private set; }
        
        public int? UserId { get; private set; }
        
        [StringLength(100)]
        public string? UserName { get; private set; }
        
        [StringLength(45)]
        public string? IpAddress { get; private set; }
        
        [StringLength(500)]
        public string? UserAgent { get; private set; }
        
        public string? StackTrace { get; private set; }
        
        public string? AdditionalData { get; private set; }

        // Navigation Properties
        public virtual User? User { get; set; }

        protected SystemLog() { }

        public SystemLog(LogLevel level, string message, string? source = null, 
                        int? userId = null, string? userName = null)
        {
            Level = level;
            SetMessage(message);
            Source = source?.Trim();
            UserId = userId;
            UserName = userName?.Trim();
        }

        public void SetMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Log mesajı boş olamaz");

            Message = message.Trim();
            UpdateTimestamp();
        }

        public void SetUserInfo(int? userId, string? userName, string? ipAddress, string? userAgent)
        {
            UserId = userId;
            UserName = userName?.Trim();
            IpAddress = ipAddress?.Trim();
            UserAgent = userAgent?.Trim();
            UpdateTimestamp();
        }

        public void SetStackTrace(string? stackTrace)
        {
            StackTrace = stackTrace?.Trim();
            UpdateTimestamp();
        }

        public void SetAdditionalData(string? additionalData)
        {
            AdditionalData = additionalData?.Trim();
            UpdateTimestamp();
        }
    }
}