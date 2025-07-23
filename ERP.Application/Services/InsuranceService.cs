using AutoMapper;
using ERP.Application.DTOs.Insurance;
using ERP.Application.Common.Models;
using ERP.Application.Interfaces.Services;
using ERP.Core.Entities;
using ERP.Core.Interfaces;

namespace ERP.Application.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InsuranceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<InsurancePolicyDto>> GetInsurancePolicyByIdAsync(int id)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetByIdAsync(id);
                if (policy == null)
                    return Result<InsurancePolicyDto>.Failure("Sigorta poliçesi bulunamadı");

                var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
                return Result<InsurancePolicyDto>.Success(policyDto);
            }
            catch (Exception ex)
            {
                return Result<InsurancePolicyDto>.Failure($"Sigorta poliçesi getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<InsurancePolicyDto>> GetInsurancePolicyByPolicyNumberAsync(string policyNumber)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetByPolicyNumberAsync(policyNumber);
                if (policy == null)
                    return Result<InsurancePolicyDto>.Failure("Sigorta poliçesi bulunamadı");

                var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
                return Result<InsurancePolicyDto>.Success(policyDto);
            }
            catch (Exception ex)
            {
                return Result<InsurancePolicyDto>.Failure($"Sigorta poliçesi getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<InsurancePolicyDto>> GetInsurancePoliciesPagedAsync(InsuranceFilterDto filter, int pageNumber, int pageSize)
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetAllAsync();

                // Filtreleme
                if (filter.VehicleId.HasValue)
                    policies = policies.Where(p => p.VehicleId == filter.VehicleId.Value);

                if (!string.IsNullOrEmpty(filter.InsuranceCompany))
                    policies = policies.Where(p => p.InsuranceCompany.Contains(filter.InsuranceCompany, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(filter.PolicyType))
                    policies = policies.Where(p => p.PolicyType.Contains(filter.PolicyType, StringComparison.OrdinalIgnoreCase));

                if (filter.IsActive.HasValue)
                    policies = policies.Where(p => p.IsActive == filter.IsActive.Value);

                if (filter.IsExpiring.HasValue && filter.IsExpiring.Value)
                    policies = policies.Where(p => p.IsExpiringSoon);

                if (filter.StartDate.HasValue)
                    policies = policies.Where(p => p.StartDate >= filter.StartDate.Value);

                if (filter.EndDate.HasValue)
                    policies = policies.Where(p => p.EndDate <= filter.EndDate.Value);

                var totalCount = policies.Count();
                var paginatedPolicies = policies
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var policyDtos = _mapper.Map<IEnumerable<InsurancePolicyDto>>(paginatedPolicies);

                return new PaginatedResult<InsurancePolicyDto>
                {
                    Items = policyDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<InsurancePolicyDto>
                {
                    Items = new List<InsurancePolicyDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = 0, 
                };
            }
        }

        public async Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsurancePoliciesByVehicleAsync(int vehicleId)
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetByVehicleAsync(vehicleId);
                var policyDtos = _mapper.Map<IEnumerable<InsurancePolicyDto>>(policies);
                return Result<IEnumerable<InsurancePolicyDto>>.Success(policyDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InsurancePolicyDto>>.Failure($"Araç sigorta poliçeleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InsurancePolicyDto>>> GetActiveInsurancePoliciesByVehicleAsync(int vehicleId)
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetActiveByVehicleAsync(vehicleId);
                var policyDtos = _mapper.Map<IEnumerable<InsurancePolicyDto>>(policies);
                return Result<IEnumerable<InsurancePolicyDto>>.Success(policyDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InsurancePolicyDto>>.Failure($"Aktif sigorta poliçeleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsurancePoliciesByCompanyAsync(string insuranceCompany)
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetByInsuranceCompanyAsync(insuranceCompany);
                var policyDtos = _mapper.Map<IEnumerable<InsurancePolicyDto>>(policies);
                return Result<IEnumerable<InsurancePolicyDto>>.Success(policyDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InsurancePolicyDto>>.Failure($"Sigorta şirketi poliçeleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InsurancePolicyDto>>> GetExpiringInsurancePoliciesAsync(int daysAhead = 30)
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetExpiringPoliciesAsync(daysAhead);
                var policyDtos = _mapper.Map<IEnumerable<InsurancePolicyDto>>(policies);
                return Result<IEnumerable<InsurancePolicyDto>>.Success(policyDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InsurancePolicyDto>>.Failure($"Süresi dolan poliçeler getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<InsurancePolicyDto>>> GetExpiredInsurancePoliciesAsync()
        {
            try
            {
                var policies = await _unitOfWork.InsurancePolicies.GetExpiredPoliciesAsync();
                var policyDtos = _mapper.Map<IEnumerable<InsurancePolicyDto>>(policies);
                return Result<IEnumerable<InsurancePolicyDto>>.Success(policyDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<InsurancePolicyDto>>.Failure($"Süresi dolmuş poliçeler getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<InsurancePolicyDto>> GetActiveInsurancePolicyByVehicleAndTypeAsync(int vehicleId, string policyType)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetActivePolicyByVehicleAndTypeAsync(vehicleId, policyType);
                if (policy == null)
                    return Result<InsurancePolicyDto>.Failure("Aktif poliçe bulunamadı");

                var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
                return Result<InsurancePolicyDto>.Success(policyDto);
            }
            catch (Exception ex)
            {
                return Result<InsurancePolicyDto>.Failure($"Aktif poliçe getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<InsurancePolicyDto>> CreateInsurancePolicyAsync(CreateInsurancePolicyDto dto)
        {
            try
            {
                // Araç kontrolü
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(dto.VehicleId);
                if (vehicle == null)
                    return Result<InsurancePolicyDto>.Failure("Araç bulunamadı");

                // Poliçe numarası kontrolü
                if (await _unitOfWork.InsurancePolicies.IsPolicyNumberExistsAsync(dto.PolicyNumber))
                    return Result<InsurancePolicyDto>.Failure("Bu poliçe numarası zaten kullanımda");

                // Aynı tip aktif poliçe kontrolü
                var existingPolicy = await _unitOfWork.InsurancePolicies.GetActivePolicyByVehicleAndTypeAsync(dto.VehicleId, dto.PolicyType);
                if (existingPolicy != null)
                    return Result<InsurancePolicyDto>.Failure($"Bu araç için aktif bir {dto.PolicyType} poliçesi zaten mevcut");

                var policy = new InsurancePolicy(
                    dto.VehicleId,
                    dto.PolicyNumber,
                    dto.InsuranceCompany,
                    dto.PolicyType,
                    dto.StartDate,
                    dto.EndDate,
                    dto.PremiumAmount,
                    dto.CoverageAmount,
                    dto.Currency ?? "TRY");

                if (!string.IsNullOrEmpty(dto.Notes))
                    policy.SetNotes(dto.Notes);

                await _unitOfWork.InsurancePolicies.AddAsync(policy);
                await _unitOfWork.SaveChangesAsync();

                var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
                return Result<InsurancePolicyDto>.Success(policyDto);
            }
            catch (Exception ex)
            {
                return Result<InsurancePolicyDto>.Failure($"Sigorta poliçesi oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<InsurancePolicyDto>> UpdateInsurancePolicyAsync(int id, UpdateInsurancePolicyDto dto)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetByIdAsync(id);
                if (policy == null)
                    return Result<InsurancePolicyDto>.Failure("Sigorta poliçesi bulunamadı");

                // Poliçe numarası kontrolü (kendisi hariç)
                if (await _unitOfWork.InsurancePolicies.IsPolicyNumberExistsAsync(dto.PolicyNumber, id))
                    return Result<InsurancePolicyDto>.Failure("Bu poliçe numarası zaten kullanımda");

                policy.SetPolicyNumber(dto.PolicyNumber);
                policy.SetInsuranceCompany(dto.InsuranceCompany);
                policy.SetDates(dto.StartDate, dto.EndDate);
                policy.SetAmounts(dto.PremiumAmount, dto.CoverageAmount);
                policy.SetNotes(dto.Notes);

                await _unitOfWork.InsurancePolicies.UpdateAsync(policy);
                await _unitOfWork.SaveChangesAsync();

                var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
                return Result<InsurancePolicyDto>.Success(policyDto);
            }
            catch (Exception ex)
            {
                return Result<InsurancePolicyDto>.Failure($"Sigorta poliçesi güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteInsurancePolicyAsync(int id)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetByIdAsync(id);
                if (policy == null)
                    return Result<bool>.Failure("Sigorta poliçesi bulunamadı");

                policy.MarkAsDeleted();
                await _unitOfWork.InsurancePolicies.UpdateAsync(policy);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Sigorta poliçesi silinirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<InsurancePolicyDto>> RenewInsurancePolicyAsync(int id, RenewInsurancePolicyDto dto)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetByIdAsync(id);
                if (policy == null)
                    return Result<InsurancePolicyDto>.Failure("Sigorta poliçesi bulunamadı");

                policy.Renew(dto.StartDate, dto.EndDate, dto.PremiumAmount, dto.CoverageAmount);

                //if (!string.IsNullOrEmpty(dto.v))
                //    policy.SetPolicyNumber(dto.n);

                await _unitOfWork.InsurancePolicies.UpdateAsync(policy);
                await _unitOfWork.SaveChangesAsync();

                var policyDto = _mapper.Map<InsurancePolicyDto>(policy);
                return Result<InsurancePolicyDto>.Success(policyDto);
            }
            catch (Exception ex)
            {
                return Result<InsurancePolicyDto>.Failure($"Sigorta poliçesi yenilenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CancelInsurancePolicyAsync(int id, string reason)
        {
            try
            {
                var policy = await _unitOfWork.InsurancePolicies.GetByIdAsync(id);
                if (policy == null)
                    return Result<bool>.Failure("Sigorta poliçesi bulunamadı");

                policy.Cancel();
                await _unitOfWork.InsurancePolicies.UpdateAsync(policy);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Sigorta poliçesi iptal edilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsPolicyNumberUniqueAsync(string policyNumber, int? excludePolicyId = null)
        {
            try
            {
                var exists = await _unitOfWork.InsurancePolicies.IsPolicyNumberExistsAsync(policyNumber, excludePolicyId);
                return Result<bool>.Success(!exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Poliçe numarası kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        // Placeholder implementations for remaining methods
        public Task<Result<bool>> ScheduleInsuranceExpiryNotificationAsync(int policyId, DateTime reminderDate) => throw new NotImplementedException();
        public Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsuranceRenewalReportAsync(DateTime? startDate = null, DateTime? endDate = null) => throw new NotImplementedException();
        public Task<Result<InsuranceStatisticsDto>> GetInsuranceStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null) => throw new NotImplementedException();
        public Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsuranceCostAnalysisAsync(InsuranceCostAnalysisFilterDto filter) => throw new NotImplementedException();
        public Task<Result<IEnumerable<InsurancePolicyDto>>> ExportInsurancePoliciesAsync(InsuranceExportFilterDto filter) => throw new NotImplementedException();
        public Task<Result<bool>> ImportInsurancePoliciesAsync(byte[] data, InsuranceImportFormat format) => throw new NotImplementedException();
    }
}