using ERP.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Insurance
{
    public class InsurancePolicyDto : BaseDto
    {
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string InsuranceCompany { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public string Currency { get; set; } = "TRY";
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public bool IsExpiringSoon { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string? Notes { get; set; }
    }
}