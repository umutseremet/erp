using ERP.Application.DTOs.Insurance;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Queries
{
    public class GetExpiringInsurancePoliciesQuery : IRequest<IEnumerable<InsurancePolicyDto>>
    {
        public int DaysAhead { get; set; } = 30;
    }
}