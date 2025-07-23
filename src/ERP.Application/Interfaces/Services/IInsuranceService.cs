using ERP.Application.DTOs.Insurance;
using ERP.Application.Common.Models;

namespace ERP.Application.Interfaces.Services
{
    public interface IInsuranceService
    {
        Task<Result<InsurancePolicyDto>> GetInsurancePolicyByIdAsync(int id);
        Task<Result<InsurancePolicyDto>> GetInsurancePolicyByPolicyNumberAsync(string policyNumber);
        Task<PaginatedResult<InsurancePolicyDto>> GetInsurancePoliciesPagedAsync(InsuranceFilterDto filter, int pageNumber, int pageSize);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsurancePoliciesByVehicleAsync(int vehicleId);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetActiveInsurancePoliciesByVehicleAsync(int vehicleId);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsurancePoliciesByCompanyAsync(string insuranceCompany);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetExpiringInsurancePoliciesAsync(int daysAhead = 30);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetExpiredInsurancePoliciesAsync();
        Task<Result<InsurancePolicyDto>> GetActiveInsurancePolicyByVehicleAndTypeAsync(int vehicleId, string policyType);
        Task<Result<InsurancePolicyDto>> CreateInsurancePolicyAsync(CreateInsurancePolicyDto dto);
        Task<Result<InsurancePolicyDto>> UpdateInsurancePolicyAsync(int id, UpdateInsurancePolicyDto dto);
        Task<Result<bool>> DeleteInsurancePolicyAsync(int id);
        Task<Result<InsurancePolicyDto>> RenewInsurancePolicyAsync(int id, RenewInsurancePolicyDto dto);
        Task<Result<bool>> CancelInsurancePolicyAsync(int id, string reason);
        Task<Result<bool>> ScheduleInsuranceExpiryNotificationAsync(int policyId, DateTime reminderDate);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsuranceRenewalReportAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<bool>> IsPolicyNumberUniqueAsync(string policyNumber, int? excludePolicyId = null);
        Task<Result<InsuranceStatisticsDto>> GetInsuranceStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<Result<IEnumerable<InsurancePolicyDto>>> GetInsuranceCostAnalysisAsync(InsuranceCostAnalysisFilterDto filter);
        Task<Result<IEnumerable<InsurancePolicyDto>>> ExportInsurancePoliciesAsync(InsuranceExportFilterDto filter);
        Task<Result<bool>> ImportInsurancePoliciesAsync(byte[] data, InsuranceImportFormat format);
    }
}