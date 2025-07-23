namespace ERP.Application.Interfaces.External
{
    /// <summary>
    /// SMS gönderimi için servis interface'i
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Tek alıcıya SMS gönderir
        /// </summary>
        Task<SmsResult> SendSmsAsync(string phoneNumber, string message, string? senderId = null);

        /// <summary>
        /// Birden fazla alıcıya SMS gönderir
        /// </summary>
        Task<SmsBulkResult> SendBulkSmsAsync(IEnumerable<SmsRecipient> recipients, string message, string? senderId = null);

        /// <summary>
        /// Zamanlanmış SMS gönderir
        /// </summary>
        Task<SmsResult> SendScheduledSmsAsync(string phoneNumber, string message, DateTime scheduledDate, string? senderId = null);

        /// <summary>
        /// OTP (One Time Password) SMS gönderir
        /// </summary>
        Task<SmsResult> SendOtpSmsAsync(string phoneNumber, string otpCode, int validityMinutes = 5);

        /// <summary>
        /// Template kullanarak SMS gönderir
        /// </summary>
        Task<SmsResult> SendTemplatedSmsAsync(string phoneNumber, string templateName, object parameters, string? senderId = null);

        /// <summary>
        /// SMS durumunu sorgular
        /// </summary>
        Task<SmsStatus> GetSmsStatusAsync(string messageId);

        /// <summary>
        /// Birden fazla SMS durumunu sorgular
        /// </summary>
        Task<IEnumerable<SmsStatusResult>> GetBulkSmsStatusAsync(IEnumerable<string> messageIds);

        /// <summary>
        /// SMS gönderimini iptal eder (henüz gönderilmemişse)
        /// </summary>
        Task<bool> CancelSmsAsync(string messageId);

        /// <summary>
        /// SMS bakiyesini sorgular
        /// </summary>
        Task<SmsBalance> GetBalanceAsync();

        /// <summary>
        /// SMS fiyat bilgisini getirir
        /// </summary>
        Task<SmsPricing> GetPricingAsync(string phoneNumber);

        /// <summary>
        /// Blacklist yönetimi
        /// </summary>
        Task<bool> AddToBlacklistAsync(string phoneNumber, string? reason = null);
        Task<bool> RemoveFromBlacklistAsync(string phoneNumber);
        Task<bool> IsBlacklistedAsync(string phoneNumber);
        Task<IEnumerable<BlacklistedNumber>> GetBlacklistAsync();

        /// <summary>
        /// SMS şablonları yönetimi
        /// </summary>
        Task<bool> CreateTemplateAsync(string templateName, string content, SmsTemplateType type = SmsTemplateType.Marketing);
        Task<bool> UpdateTemplateAsync(string templateName, string content);
        Task<bool> DeleteTemplateAsync(string templateName);
        Task<SmsTemplate?> GetTemplateAsync(string templateName);
        Task<IEnumerable<SmsTemplate>> GetAllTemplatesAsync();

        /// <summary>
        /// SMS istatistikleri
        /// </summary>
        Task<SmsStatistics> GetStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gelen SMS'leri alır (varsa)
        /// </summary>
        Task<IEnumerable<IncomingSms>> GetIncomingSmsAsync(DateTime? since = null);

        /// <summary>
        /// SMS provider bağlantısını test eder
        /// </summary>
        Task<bool> TestConnectionAsync();

        /// <summary>
        /// Telefon numarası doğrulaması
        /// </summary>
        Task<PhoneNumberValidation> ValidatePhoneNumberAsync(string phoneNumber);
    }

    #region Models and DTOs

    /// <summary>
    /// SMS gönderim sonucu
    /// </summary>
    public class SmsResult
    {
        public bool IsSuccess { get; set; }
        public string MessageId { get; set; } = string.Empty;
        public string? ErrorMessage { get; set; }
        public string? ErrorCode { get; set; }
        public decimal? Cost { get; set; }
        public int MessageParts { get; set; } = 1;
        public DateTime SentAt { get; set; }
    }

    /// <summary>
    /// SMS alıcısı bilgileri
    /// </summary>
    public class SmsRecipient
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Name { get; set; }
        public Dictionary<string, object>? PersonalizationData { get; set; }
    }

    /// <summary>
    /// Toplu SMS gönderim sonucu
    /// </summary>
    public class SmsBulkResult
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<SmsFailure> Failures { get; set; } = new List<SmsFailure>();
        public decimal TotalCost { get; set; }
        public bool IsSuccess => FailureCount == 0;
        public decimal SuccessRate => TotalCount > 0 ? (decimal)SuccessCount / TotalCount * 100 : 0;
    }

    /// <summary>
    /// SMS gönderim hatası
    /// </summary>
    public class SmsFailure
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// SMS durumu
    /// </summary>
    public enum SmsStatus
    {
        Queued = 1,
        Sent = 2,
        Delivered = 3,
        Failed = 4,
        Expired = 5,
        Rejected = 6,
        Unknown = 7
    }

    /// <summary>
    /// SMS durum sonucu
    /// </summary>
    public class SmsStatusResult
    {
        public string MessageId { get; set; } = string.Empty;
        public SmsStatus Status { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal? Cost { get; set; }
    }

    /// <summary>
    /// SMS bakiye bilgisi
    /// </summary>
    public class SmsBalance
    {
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "TRY";
        public int EstimatedSmsCount { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// SMS fiyat bilgisi
    /// </summary>
    public class SmsPricing
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string OperatorName { get; set; } = string.Empty;
        public decimal PricePerSms { get; set; }
        public string Currency { get; set; } = "TRY";
    }

    /// <summary>
    /// Kara liste numarası
    /// </summary>
    public class BlacklistedNumber
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public DateTime AddedAt { get; set; }
        public string AddedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// SMS şablon türü
    /// </summary>
    public enum SmsTemplateType
    {
        Marketing = 1,
        Transactional = 2,
        OTP = 3,
        Alert = 4,
        Reminder = 5
    }

    /// <summary>
    /// SMS şablonu
    /// </summary>
    public class SmsTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public SmsTemplateType Type { get; set; }
        public List<string> Variables { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// SMS istatistikleri
    /// </summary>
    public class SmsStatistics
    {
        public int TotalSent { get; set; }
        public int TotalDelivered { get; set; }
        public int TotalFailed { get; set; }
        public decimal TotalCost { get; set; }
        public string Currency { get; set; } = "TRY";
        public decimal DeliveryRate => TotalSent > 0 ? (decimal)TotalDelivered / TotalSent * 100 : 0;
        public decimal FailureRate => TotalSent > 0 ? (decimal)TotalFailed / TotalSent * 100 : 0;
        public decimal AverageCostPerSms => TotalSent > 0 ? TotalCost / TotalSent : 0;
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }

    /// <summary>
    /// Gelen SMS
    /// </summary>
    public class IncomingSms
    {
        public string MessageId { get; set; } = string.Empty;
        public string FromNumber { get; set; } = string.Empty;
        public string ToNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
        public bool IsProcessed { get; set; }
    }

    /// <summary>
    /// Telefon numarası doğrulama sonucu
    /// </summary>
    public class PhoneNumberValidation
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public string? FormattedNumber { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? OperatorName { get; set; }
        public PhoneNumberType Type { get; set; }
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Telefon numarası türü
    /// </summary>
    public enum PhoneNumberType
    {
        Unknown = 0,
        Mobile = 1,
        Landline = 2,
        VoIP = 3,
        Premium = 4,
        TollFree = 5
    }

    #endregion
}