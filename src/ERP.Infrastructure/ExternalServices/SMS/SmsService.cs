using Microsoft.Extensions.Options;
using ERP.Infrastructure.ExternalServices.SMS.Models;
using System.Text;
using System.Text.Json;

namespace ERP.Infrastructure.ExternalServices.SMS
{
    public class SmsService : ISmsService
    {
        private readonly HttpClient _httpClient;
        private readonly SmsSettings _settings;

        public SmsService(HttpClient httpClient, IOptions<SmsSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            // HttpClient base address ve timeout ayarlarý
            if (!string.IsNullOrEmpty(_settings.ApiEndpoint))
            {
                var baseUri = new Uri(_settings.ApiEndpoint);
                _httpClient.BaseAddress = new Uri($"{baseUri.Scheme}://{baseUri.Host}:{baseUri.Port}");
            }

            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<bool> SendSmsAsync(SmsMessage message)
        {
            try
            {
                var payload = new
                {
                    username = _settings.Username,
                    password = _settings.Password,
                    to = message.PhoneNumber,
                    message = message.Message,
                    from = _settings.SenderName
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_settings.ApiEndpoint, content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            var smsMessage = new SmsMessage
            {
                PhoneNumber = phoneNumber,
                Message = message
            };

            return await SendSmsAsync(smsMessage);
        }

        public async Task<bool> SendBulkSmsAsync(IEnumerable<SmsMessage> messages)
        {
            var tasks = messages.Select(SendSmsAsync);
            var results = await Task.WhenAll(tasks);

            return results.All(r => r);
        }

        public Task<bool> IsPhoneNumberValidAsync(string phoneNumber)
        {
            // Türkiye telefon numarasý formatý kontrolü
            var cleanNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            var isValid = (cleanNumber.Length == 10 && cleanNumber.StartsWith("5")) ||
                         (cleanNumber.Length == 12 && cleanNumber.StartsWith("905"));

            return Task.FromResult(isValid);
        }
    }
}