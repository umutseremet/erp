using ERP.Application.DTOs.Insurance;
using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Queries
{
    public class GetInsurancePolicyByIdQuery : IRequest<Result<InsurancePolicyDto>>
    {
        public int Id { get; set; }
    }
}