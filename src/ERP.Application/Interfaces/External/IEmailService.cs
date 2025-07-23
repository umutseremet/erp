namespace ERP.Application.Interfaces.External
{
    /// <summary>
    /// Email gönderimi için servis interface'i
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Tek alıcıya email gönderir
        /// </summary>
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);

        /// <summary>
        /// Birden fazla alıcıya email gönderir
        /// </summary>
        Task<bool> SendEmailAsync(IEnumerable<string> to, string subject, string body, bool isHtml = true);

        /// <summary>
        /// CC ve BCC ile email gönderir
        /// </summary>
        Task<bool> SendEmailAsync(string to, string subject, string body, IEnumerable<string>? cc = null, IEnumerable<string>? bcc = null, bool isHtml = true);

        /// <summary>
        /// Ek dosyalar ile email gönderir
        /// </summary>
        Task<bool> SendEmailWithAttachmentsAsync(string to, string subject, string body, IEnumerable<EmailAttachment> attachments, bool isHtml = true);

        /// <summary>
        /// Template kullanarak email gönderir
        /// </summary>
        Task<bool> SendTemplatedEmailAsync(string to, string templateName, object model);

        /// <summary>
        /// Toplu email gönderimi (bulk email)
        /// </summary>
        Task<EmailBulkResult> SendBulkEmailAsync(IEnumerable<EmailRecipient> recipients, string subject, string body, bool isHtml = true);

        /// <summary>
        /// Email gönderim durumunu kontrol eder
        /// </summary>
        Task<EmailStatus> GetEmailStatusAsync(string messageId);

        /// <summary>
        /// Email şablonlarını yönetir
        /// </summary>
        Task<bool> CreateTemplateAsync(string templateName, string subject, string body);
        Task<bool> UpdateTemplateAsync(string templateName, string subject, string body);
        Task<bool> DeleteTemplateAsync(string templateName);
        Task<EmailTemplate?> GetTemplateAsync(string templateName);
        Task<IEnumerable<EmailTemplate>> GetAllTemplatesAsync();

        /// <summary>
        /// Email kuyruğu yönetimi
        /// </summary>
        Task<string> QueueEmailAsync(string to, string subject, string body, DateTime? scheduledDate = null);
        Task<bool> CancelQueuedEmailAsync(string messageId);
        Task<IEnumerable<QueuedEmail>> GetQueuedEmailsAsync();

        /// <summary>
        /// Email istatistikleri
        /// </summary>
        Task<EmailStatistics> GetEmailStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Email yapılandırma testi
        /// </summary>
        Task<bool> TestConnectionAsync();
    }

    /// <summary>
    /// Email eki için model
    /// </summary>
    public class EmailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }

    /// <summary>
    /// Email alıcısı bilgileri
    /// </summary>
    public class EmailRecipient
    {
        public string Email { get; set; } = string.Empty;
        public string? Name { get; set; }
        public Dictionary<string, object>? PersonalizationData { get; set; }
    }

    /// <summary>
    /// Toplu email gönderim sonucu
    /// </summary>
    public class EmailBulkResult
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<EmailFailure> Failures { get; set; } = new List<EmailFailure>();
        public bool IsSuccess => FailureCount == 0;
        public decimal SuccessRate => TotalCount > 0 ? (decimal)SuccessCount / TotalCount * 100 : 0;
    }

    /// <summary>
    /// Email gönderim hatası
    /// </summary>
    public class EmailFailure
    {
        public string Email { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// Email durumu
    /// </summary>
    public enum EmailStatus
    {
        Queued = 1,
        Sending = 2,
        Sent = 3,
        Delivered = 4,
        Opened = 5,
        Clicked = 6,
        Failed = 7,
        Bounced = 8,
        Spam = 9,
        Unsubscribed = 10
    }

    /// <summary>
    /// Email şablonu
    /// </summary>
    public class EmailTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Variables { get; set; } = new List<string>();
    }

    /// <summary>
    /// Kuyruktaki email
    /// </summary>
    public class QueuedEmail
    {
        public string Id { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime QueuedAt { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public EmailStatus Status { get; set; }
        public int RetryCount { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Email istatistikleri
    /// </summary>
    public class EmailStatistics
    {
        public int TotalSent { get; set; }
        public int TotalDelivered { get; set; }
        public int TotalOpened { get; set; }
        public int TotalClicked { get; set; }
        public int TotalFailed { get; set; }
        public int TotalBounced { get; set; }
        public decimal DeliveryRate => TotalSent > 0 ? (decimal)TotalDelivered / TotalSent * 100 : 0;
        public decimal OpenRate => TotalDelivered > 0 ? (decimal)TotalOpened / TotalDelivered * 100 : 0;
        public decimal ClickRate => TotalOpened > 0 ? (decimal)TotalClicked / TotalOpened * 100 : 0;
        public decimal BounceRate => TotalSent > 0 ? (decimal)TotalBounced / TotalSent * 100 : 0;
    }
}