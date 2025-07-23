namespace ERP.Application.DTOs.Insurance
{
    public class RenewInsurancePolicyDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public string? Notes { get; set; }
    }
}