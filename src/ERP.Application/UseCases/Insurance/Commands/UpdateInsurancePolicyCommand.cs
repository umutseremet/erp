using ERP.Application.Common.Models;
using ERP.Application.DTOs.Insurance;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Commands
{
    public class UpdateInsurancePolicyCommand : IRequest<Result<InsurancePolicyDto>>
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string InsuranceCompany { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public string Currency { get; set; } = "TRY";
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}