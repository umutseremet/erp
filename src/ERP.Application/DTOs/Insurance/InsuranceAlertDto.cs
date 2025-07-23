namespace ERP.Application.DTOs.Insurance
{
    public class InsuranceAlertDto
    {
        public int PolicyId { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string InsuranceCompany { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string AlertLevel { get; set; } = string.Empty; // Warning, Critical
        public string Message { get; set; } = string.Empty;
        public decimal PremiumAmount { get; set; }
        public string Currency { get; set; } = "TRY";
    }
}