using ERP.Application.DTOs.Insurance;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Queries
{
    public class GetInsurancePoliciesByVehicleQuery : IRequest<IEnumerable<InsurancePolicyDto>>
    {
        public int VehicleId { get; set; }
        public bool OnlyActive { get; set; } = false;
    }
}