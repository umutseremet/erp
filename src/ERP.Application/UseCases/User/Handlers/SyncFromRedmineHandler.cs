using ERP.Application.Common.Models;
using ERP.Application.Interfaces.Services;
using ERP.Application.UseCases.User.Commands;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class SyncFromRedmineHandler : IRequestHandler<SyncFromRedmineCommand, Result<bool>>
    {
        private readonly IRedmineIntegrationService _redmineService;

        public SyncFromRedmineHandler(IRedmineIntegrationService redmineService)
        {
            _redmineService = redmineService;
        }

        public async Task<Result<bool>> Handle(SyncFromRedmineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _redmineService.SyncUsersFromRedmineAsync();
                return result;
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Redmine senkronizasyonu sırasında hata: {ex.Message}");
            }
        }
    }
}