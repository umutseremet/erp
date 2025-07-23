using ERP.Infrastructure.ExternalServices.SMS.Models;

namespace ERP.Infrastructure.ExternalServices.SMS
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(SmsMessage message);
        Task<bool> SendSmsAsync(string phoneNumber, string message);
        Task<bool> SendBulkSmsAsync(IEnumerable<SmsMessage> messages);
        Task<bool> IsPhoneNumberValidAsync(string phoneNumber);
    }
}