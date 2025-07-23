using ERP.Infrastructure.ExternalServices.Email.Models;
using System.Net.Mail;

namespace ERP.Infrastructure.ExternalServices.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendBulkEmailAsync(IEnumerable<EmailMessage> messages);
        Task<bool> IsEmailValidAsync(string email);
    }
}