namespace ERP.Infrastructure.ExternalServices.SMS.Models
{
    public class SmsSettings
    {
        public string ApiEndpoint { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
    }
}