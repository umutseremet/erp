using System.ComponentModel.DataAnnotations;
using ERP.Core.ValueObjects;

namespace ERP.Core.Entities
{
    public class InsurancePolicy : BaseEntity
    {
        public int VehicleId { get; private set; }
        
        [Required]
        [StringLength(100)]
        public string PolicyNumber { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string InsuranceCompany { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string PolicyType { get; private set; } = string.Empty; // Kasko, Trafik
        
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        
        public decimal PremiumAmount { get; private set; }
        public decimal CoverageAmount { get; private set; }
        
        [StringLength(50)]
        public string Currency { get; private set; } = "TRY";
        
        public bool IsActive { get; private set; } = true;
        
        [StringLength(1000)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;

        protected InsurancePolicy() { }

        public InsurancePolicy(int vehicleId, string policyNumber, string insuranceCompany, 
                             string policyType, DateTime startDate, DateTime endDate, 
                             decimal premiumAmount, decimal coverageAmount, string currency = "TRY")
        {
            VehicleId = vehicleId;
            SetPolicyNumber(policyNumber);
            SetInsuranceCompany(insuranceCompany);
            SetPolicyType(policyType);
            SetDates(startDate, endDate);
            SetAmounts(premiumAmount, coverageAmount);
            Currency = currency;
        }

        public void SetPolicyNumber(string policyNumber)
        {
            if (string.IsNullOrWhiteSpace(policyNumber))
                throw new ArgumentException("Poliçe numarası boş olamaz");

            PolicyNumber = policyNumber.Trim();
            UpdateTimestamp();
        }

        public void SetInsuranceCompany(string insuranceCompany)
        {
            if (string.IsNullOrWhiteSpace(insuranceCompany))
                throw new ArgumentException("Sigorta şirketi boş olamaz");

            InsuranceCompany = insuranceCompany.Trim();
            UpdateTimestamp();
        }

        public void SetPolicyType(string policyType)
        {
            if (string.IsNullOrWhiteSpace(policyType))
                throw new ArgumentException("Poliçe türü boş olamaz");

            PolicyType = policyType.Trim();
            UpdateTimestamp();
        }

        public void SetDates(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
                throw new ArgumentException("Bitiş tarihi başlangıç tarihinden sonra olmalıdır");

            StartDate = startDate;
            EndDate = endDate;
            UpdateTimestamp();
        }

        public void SetAmounts(decimal premiumAmount, decimal coverageAmount)
        {
            if (premiumAmount <= 0)
                throw new ArgumentException("Prim tutarı pozitif olmalıdır");
            
            if (coverageAmount <= 0)
                throw new ArgumentException("Teminat tutarı pozitif olmalıdır");

            PremiumAmount = premiumAmount;
            CoverageAmount = coverageAmount;
            UpdateTimestamp();
        }

        public void Renew(DateTime newStartDate, DateTime newEndDate, decimal newPremiumAmount, decimal newCoverageAmount)
        {
            SetDates(newStartDate, newEndDate);
            SetAmounts(newPremiumAmount, newCoverageAmount);
            IsActive = true;
            UpdateTimestamp();
        }

        public void Cancel()
        {
            IsActive = false;
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public bool IsExpired => DateTime.UtcNow > EndDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > EndDate;
        public int DaysUntilExpiry => (EndDate - DateTime.UtcNow).Days;

        public Money GetPremiumAsMoney()
        {
            return new Money(PremiumAmount, Currency);
        }

        public Money GetCoverageAsMoney()
        {
            return new Money(CoverageAmount, Currency);
        }
    }
}