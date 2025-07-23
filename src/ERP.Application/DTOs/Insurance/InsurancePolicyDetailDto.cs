namespace ERP.Application.DTOs.Insurance
{
    public class InsurancePolicyDetailDto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string InsuranceCompany { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public decimal CoverageAmount { get; set; }
        public string Currency { get; set; } = "TRY";
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsExpired => DateTime.UtcNow > EndDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > EndDate;
        public int DaysUntilExpiry => (EndDate - DateTime.UtcNow).Days;
        public string ValidityPeriod => $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";
        public string FormattedPremium => $"{PremiumAmount:C} {Currency}";
        public string FormattedCoverage => $"{CoverageAmount:C} {Currency}";
        public string StatusText => IsActive ? (IsExpired ? "Süresi Dolmuş" : "Aktif") : "Pasif";
    }
}