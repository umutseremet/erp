using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Insurance;
using ERP.Application.UseCases.Insurance.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Insurance.Handlers
{
    public class GetInsurancePolicyByIdHandler : IRequestHandler<GetInsurancePolicyByIdQuery, Result<InsurancePolicyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInsurancePolicyByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<InsurancePolicyDto>> Handle(GetInsurancePolicyByIdQuery request, CancellationToken cancellationToken)
        {
            var policy = await _unitOfWork.InsurancePolicies.GetByIdAsync(request.Id);
            if (policy == null)
            {
                return Result<InsurancePolicyDto>.Failure("Sigorta poliçesi bulunamadı.");
            }

            var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
            return Result<InsurancePolicyDto>.Success(policyDto);
        }
    }

    public class GetInsurancePoliciesByVehicleHandler : IRequestHandler<GetInsurancePoliciesByVehicleQuery, IEnumerable<InsurancePolicyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInsurancePoliciesByVehicleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InsurancePolicyDto>> Handle(GetInsurancePoliciesByVehicleQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ERP.Core.Entities.InsurancePolicy> policies;

            if (request.OnlyActive)
            {
                policies = await _unitOfWork.InsurancePolicies.GetActiveByVehicleAsync(request.VehicleId);
            }
            else
            {
                policies = await _unitOfWork.InsurancePolicies.GetByVehicleAsync(request.VehicleId);
            }

            return _mapper.Map<IEnumerable<InsurancePolicyDto>>(policies);
        }
    }

    public class GetExpiringInsurancePoliciesHandler : IRequestHandler<GetExpiringInsurancePoliciesQuery, IEnumerable<InsurancePolicyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetExpiringInsurancePoliciesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InsurancePolicyDto>> Handle(GetExpiringInsurancePoliciesQuery request, CancellationToken cancellationToken)
        {
            var expiringPolicies = await _unitOfWork.InsurancePolicies.GetExpiringPoliciesAsync(request.DaysAhead);
            return _mapper.Map<IEnumerable<InsurancePolicyDto>>(expiringPolicies);
        }
    }
}