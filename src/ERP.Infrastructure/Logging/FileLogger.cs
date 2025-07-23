using Microsoft.Extensions.Logging;
using ERP.Core.Enums;
using System.Text.Json;

namespace ERP.Infrastructure.Logging
{
    public class FileLogger : ICustomLogger
    {
        private readonly string _logPath;
        private readonly ILogger<FileLogger> _logger;

        public FileLogger(ILogger<FileLogger> logger, string logPath = "logs")
        {
            _logger = logger;
            _logPath = logPath;

            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
        }

        public async Task LogAsync(Core.Enums.LogLevel level, string message, string? source = null, int? userId = null, string? userName = null)
        {
            try
            {
                var logEntry = new
                {
                    Timestamp = DateTime.UtcNow,
                    Level = level.ToString(),
                    Message = message,
                    Source = source,
                    UserId = userId,
                    UserName = userName
                };

                var logJson = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions { WriteIndented = true });
                var fileName = $"log_{DateTime.UtcNow:yyyy-MM-dd}.json";
                var filePath = Path.Combine(_logPath, fileName);

                await File.AppendAllTextAsync(filePath, logJson + Environment.NewLine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write to log file");
            }
        }

        public async Task LogInformationAsync(string message, string? source = null, int? userId = null)
        {
            await LogAsync(Core.Enums.LogLevel.Information, message, source, userId);
        }

        public async Task LogWarningAsync(string message, string? source = null, int? userId = null)
        {
            await LogAsync(Core.Enums.LogLevel.Warning, message, source, userId);
        }

        public async Task LogErrorAsync(string message, Exception? exception = null, string? source = null, int? userId = null)
        {
            var fullMessage = exception != null ? $"{message} - Exception: {exception.Message}" : message;
            await LogAsync(Core.Enums.LogLevel.Error, fullMessage, source, userId);
        }

        public async Task LogCriticalAsync(string message, Exception? exception = null, string? source = null, int? userId = null)
        {
            var fullMessage = exception != null ? $"{message} - Exception: {exception.Message}" : message;
            await LogAsync(Core.Enums.LogLevel.Critical, fullMessage, source, userId);
        }
    }
}