using ERP.Core.Enums;

namespace ERP.Infrastructure.Logging
{
    public interface ICustomLogger
    {
        Task LogAsync(LogLevel level, string message, string? source = null, int? userId = null, string? userName = null);
        Task LogInformationAsync(string message, string? source = null, int? userId = null);
        Task LogWarningAsync(string message, string? source = null, int? userId = null);
        Task LogErrorAsync(string message, Exception? exception = null, string? source = null, int? userId = null);
        Task LogCriticalAsync(string message, Exception? exception = null, string? source = null, int? userId = null);
    }
}