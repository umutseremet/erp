using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Commands
{
    public class ReturnVehicleCommand : IRequest<Result<bool>>
    {
        public int VehicleId { get; set; }
        public decimal? CurrentKm { get; set; }
        public string? Notes { get; set; }
    }
}