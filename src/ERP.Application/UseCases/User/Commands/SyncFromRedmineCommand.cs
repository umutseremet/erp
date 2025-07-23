using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.User.Commands
{
    public class SyncFromRedmineCommand : IRequest<Result<bool>>
    {
        public bool ForceSync { get; set; }
        public List<int>? SpecificUserIds { get; set; }
    }
}