using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class InsurancePolicyRepository : BaseRepository<InsurancePolicy>, IInsurancePolicyRepository
    {
        public InsurancePolicyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<InsurancePolicy?> GetByPolicyNumberAsync(string policyNumber)
        {
            return await _dbSet
                .Include(ip => ip.Vehicle)
                .FirstOrDefaultAsync(ip => ip.PolicyNumber == policyNumber);
        }

        public async Task<IEnumerable<InsurancePolicy>> GetByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(ip => ip.Vehicle)
                .Where(ip => ip.VehicleId == vehicleId)
                .OrderByDescending(ip => ip.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsurancePolicy>> GetActiveByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Include(ip => ip.Vehicle)
                .Where(ip => ip.VehicleId == vehicleId && ip.IsActive)
                .OrderByDescending(ip => ip.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsurancePolicy>> GetByInsuranceCompanyAsync(string insuranceCompany)
        {
            return await _dbSet
                .Include(ip => ip.Vehicle)
                .Where(ip => ip.InsuranceCompany == insuranceCompany)
                .OrderByDescending(ip => ip.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsurancePolicy>> GetExpiringPoliciesAsync(int daysAhead = 30)
        {
            var targetDate = DateTime.UtcNow.AddDays(daysAhead);

            return await _dbSet
                .Include(ip => ip.Vehicle)
                .Where(ip => ip.IsActive && ip.EndDate <= targetDate)
                .OrderBy(ip => ip.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InsurancePolicy>> GetExpiredPoliciesAsync()
        {
            return await _dbSet
                .Include(ip => ip.Vehicle)
                .Where(ip => ip.IsActive && ip.EndDate < DateTime.UtcNow)
                .OrderBy(ip => ip.EndDate)
                .ToListAsync();
        }

        public async Task<InsurancePolicy?> GetActivePolicyByVehicleAndTypeAsync(int vehicleId, string policyType)
        {
            return await _dbSet
                .Include(ip => ip.Vehicle)
                .FirstOrDefaultAsync(ip => ip.VehicleId == vehicleId &&
                                         ip.PolicyType == policyType &&
                                         ip.IsActive &&
                                         ip.EndDate > DateTime.UtcNow);
        }

        public async Task<bool> IsPolicyNumberExistsAsync(string policyNumber, int? excludeId = null)
        {
            var query = _dbSet.Where(ip => ip.PolicyNumber == policyNumber);

            if (excludeId.HasValue)
            {
                query = query.Where(ip => ip.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}