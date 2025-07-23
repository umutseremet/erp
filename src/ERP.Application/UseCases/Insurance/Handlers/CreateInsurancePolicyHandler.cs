using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Insurance;
using ERP.Application.UseCases.Insurance.Commands;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Handlers
{
    public class CreateInsurancePolicyHandler : IRequestHandler<CreateInsurancePolicyCommand, Result<InsurancePolicyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateInsurancePolicyHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<InsurancePolicyDto>> Handle(CreateInsurancePolicyCommand request, CancellationToken cancellationToken)
        {
            // Vehicle kontrolü
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                return Result<InsurancePolicyDto>.Failure("Araç bulunamadı.");
            }

            // Poliçe numarası kontrolü
            var policyExists = await _unitOfWork.InsurancePolicies.IsPolicyNumberExistsAsync(request.PolicyNumber);
            if (policyExists)
            {
                return Result<InsurancePolicyDto>.Failure("Bu poliçe numarası zaten kullanılıyor.");
            }

            var insurancePolicy = new InsurancePolicy(
                request.VehicleId,
                request.PolicyNumber,
                request.InsuranceCompany,
                request.PolicyType,
                request.StartDate,
                request.EndDate,
                request.PremiumAmount,
                request.CoverageAmount,
                request.Currency
            );

            if (!string.IsNullOrEmpty(request.Notes))
            {
                insurancePolicy.SetNotes(request.Notes);
            }

            await _unitOfWork.InsurancePolicies.AddAsync(insurancePolicy);
            await _unitOfWork.SaveChangesAsync();

            var policyDto = _mapper.Map<InsurancePolicyDto>(insurancePolicy);
            return Result<InsurancePolicyDto>.Success(policyDto);
        }
    }
}