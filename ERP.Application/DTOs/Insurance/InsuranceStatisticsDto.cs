namespace ERP.Application.DTOs.Insurance
{
    public class InsuranceStatisticsDto
    {
        public int TotalPolicies { get; set; }
        public int ActivePolicies { get; set; }
        public int ExpiringPolicies { get; set; }
        public int ExpiredPolicies { get; set; }
        public decimal TotalPremiumAmount { get; set; }
        public decimal TotalCoverageAmount { get; set; }
        public Dictionary<string, int> PoliciesByCompany { get; set; } = new();
        public Dictionary<string, int> PoliciesByType { get; set; } = new();
    }
}