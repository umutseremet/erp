using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Insurance
{
    public class CreateInsurancePolicyDto
    {
        public int VehicleId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string InsuranceCompany { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public string Currency { get; set; } = "TRY";
        public string? Notes { get; set; }
    }
}