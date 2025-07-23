using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Queries
{
    public class GetVehiclesByUserQuery : IRequest<Result<IEnumerable<VehicleDto>>>
    {
        public int UserId { get; set; }
        public bool IncludeHistory { get; set; } = false;
    }
}