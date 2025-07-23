using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using ERP.Core.Enums;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Queries
{
    public class GetAvailableVehiclesQuery : IRequest<Result<IEnumerable<VehicleDto>>>
    {
        public VehicleType? Type { get; set; }
        public bool IncludeAssigned { get; set; } = false;
    }
}