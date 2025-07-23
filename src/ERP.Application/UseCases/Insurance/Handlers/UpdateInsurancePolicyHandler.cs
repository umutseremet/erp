using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Insurance;
using ERP.Application.UseCases.Insurance.Commands;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Handlers
{
    public class UpdateInsurancePolicyHandler : IRequestHandler<UpdateInsurancePolicyCommand, Result<InsurancePolicyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateInsurancePolicyHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<InsurancePolicyDto>> Handle(UpdateInsurancePolicyCommand request, CancellationToken cancellationToken)
        {
            var insurancePolicy = await _unitOfWork.InsurancePolicies.GetByIdAsync(request.Id);
            if (insurancePolicy == null)
            {
                return Result<InsurancePolicyDto>.Failure("Sigorta poliçesi bulunamadı.");
            }

            // Poliçe numarası değişikliği kontrolü
            if (insurancePolicy.PolicyNumber != request.PolicyNumber)
            {
                var policyExists = await _unitOfWork.InsurancePolicies.IsPolicyNumberExistsAsync(request.PolicyNumber, request.Id);
                if (policyExists)
                {
                    return Result<InsurancePolicyDto>.Failure("Bu poliçe numarası zaten kullanılıyor.");
                }
            }

            insurancePolicy.SetPolicyNumber(request.PolicyNumber);
            insurancePolicy.SetInsuranceCompany(request.InsuranceCompany);
            insurancePolicy.SetPolicyType(request.PolicyType);
            insurancePolicy.SetDates(request.StartDate, request.EndDate);
            insurancePolicy.SetAmounts(request.PremiumAmount, request.CoverageAmount);
            insurancePolicy.SetNotes(request.Notes);

            if (!request.IsActive)
                insurancePolicy.Cancel();

            await _unitOfWork.InsurancePolicies.UpdateAsync(insurancePolicy);
            await _unitOfWork.SaveChangesAsync();

            var policyDto = _mapper.Map<InsurancePolicyDto>(insurancePolicy);
            return Result<InsurancePolicyDto>.Success(policyDto);
        }
    }
}