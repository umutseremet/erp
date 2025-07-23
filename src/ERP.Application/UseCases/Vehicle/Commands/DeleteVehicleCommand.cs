using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Commands
{
    public class DeleteVehicleCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}