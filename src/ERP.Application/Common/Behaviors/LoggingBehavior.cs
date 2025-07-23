using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ERP.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("ERP Request: {RequestName} started at {StartTime}",
                requestName, DateTime.UtcNow);

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation("ERP Request: {RequestName} completed successfully in {ElapsedTime}ms",
                    requestName, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex, "ERP Request: {RequestName} failed after {ElapsedTime}ms. Error: {ErrorMessage}",
                    requestName, stopwatch.ElapsedMilliseconds, ex.Message);

                throw;
            }
        }
    }
}