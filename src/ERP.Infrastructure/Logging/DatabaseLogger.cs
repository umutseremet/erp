using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;

namespace ERP.Infrastructure.Logging
{
    public class DatabaseLogger : ICustomLogger
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseLogger(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogAsync(LogLevel level, string message, string? source = null, int? userId = null, string? userName = null)
        {
            try
            {
                var log = new SystemLog(level, message, source, userId, userName);
                await _unitOfWork.Repository<SystemLog>().AddAsync(log);
                await _unitOfWork.SaveChangesAsync();
            }
            catch
            {
                // Logging hatasý durumunda sessizce devam et
            }
        }

        public async Task LogInformationAsync(string message, string? source = null, int? userId = null)
        {
            await LogAsync(LogLevel.Information, message, source, userId);
        }

        public async Task LogWarningAsync(string message, string? source = null, int? userId = null)
        {
            await LogAsync(LogLevel.Warning, message, source, userId);
        }

        public async Task LogErrorAsync(string message, Exception? exception = null, string? source = null, int? userId = null)
        {
            var fullMessage = exception != null ? $"{message} - Exception: {exception.Message}" : message;
            var log = new SystemLog(LogLevel.Error, fullMessage, source, userId);

            if (exception != null)
            {
                log.SetStackTrace(exception.StackTrace);
            }

            await _unitOfWork.Repository<SystemLog>().AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task LogCriticalAsync(string message, Exception? exception = null, string? source = null, int? userId = null)
        {
            var fullMessage = exception != null ? $"{message} - Exception: {exception.Message}" : message;
            var log = new SystemLog(LogLevel.Critical, fullMessage, source, userId);

            if (exception != null)
            {
                log.SetStackTrace(exception.StackTrace);
            }

            await _unitOfWork.Repository<SystemLog>().AddAsync(log);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}