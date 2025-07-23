using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using ERP.Infrastructure.ExternalServices.Email.Models;

namespace ERP.Infrastructure.ExternalServices.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var mimeMessage = CreateMimeMessage(message);
            await SendAsync(mimeMessage);
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            await SendEmailAsync(message);
        }

        public async Task SendBulkEmailAsync(IEnumerable<EmailMessage> messages)
        {
            var tasks = messages.Select(SendEmailAsync);
            await Task.WhenAll(tasks);
        }

        public Task<bool> IsEmailValidAsync(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return Task.FromResult(mailAddress.Address == email);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        private MimeMessage CreateMimeMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(message.To));
            mimeMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();

            if (message.IsHtml)
            {
                bodyBuilder.HtmlBody = message.Body;
            }
            else
            {
                bodyBuilder.TextBody = message.Body;
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();
            return mimeMessage;
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, _settings.UseSSL);

            if (!string.IsNullOrEmpty(_settings.Username))
            {
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
            }

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}