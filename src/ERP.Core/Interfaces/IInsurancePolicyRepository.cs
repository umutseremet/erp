using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IInsurancePolicyRepository : IRepository<InsurancePolicy>
    {
        Task<InsurancePolicy?> GetByPolicyNumberAsync(string policyNumber);
        Task<IEnumerable<InsurancePolicy>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<InsurancePolicy>> GetActiveByVehicleAsync(int vehicleId);
        Task<IEnumerable<InsurancePolicy>> GetByInsuranceCompanyAsync(string insuranceCompany);
        Task<IEnumerable<InsurancePolicy>> GetExpiringPoliciesAsync(int daysAhead = 30);
        Task<IEnumerable<InsurancePolicy>> GetExpiredPoliciesAsync();
        Task<InsurancePolicy?> GetActivePolicyByVehicleAndTypeAsync(int vehicleId, string policyType);
        Task<bool> IsPolicyNumberExistsAsync(string policyNumber, int? excludeId = null);
    }
}