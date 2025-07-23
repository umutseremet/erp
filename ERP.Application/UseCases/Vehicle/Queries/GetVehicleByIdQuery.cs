using ERP.Application.Common.Models;
using ERP.Application.DTOs.Vehicle;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Queries
{
    public class GetVehicleByIdQuery : IRequest<Result<VehicleDetailDto?>>
    {
        public int Id { get; set; }
    }
}