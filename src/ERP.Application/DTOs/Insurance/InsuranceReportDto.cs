
// 4. DTOs/Insurance/InsuranceReportDto.cs
namespace ERP.Application.DTOs.Insurance
{
    /// <summary>
    /// Sigorta raporu için DTO
    /// </summary>
    public class InsuranceReportDto
    {
        /// <summary>
        /// Rapor tarihi
        /// </summary>
        public DateTime ReportDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Toplam poliçe sayısı
        /// </summary>
        public int TotalPolicies { get; set; }

        /// <summary>
        /// Aktif poliçe sayısı
        /// </summary>
        public int ActivePolicies { get; set; }

        /// <summary>
        /// Süresi dolmuş poliçe sayısı
        /// </summary>
        public int ExpiredPolicies { get; set; }

        /// <summary>
        /// Yakında dolacak poliçe sayısı (30 gün)
        /// </summary>
        public int ExpiringSoonPolicies { get; set; }

        /// <summary>
        /// Toplam prim tutarı
        /// </summary>
        public decimal TotalPremiumAmount { get; set; }

        /// <summary>
        /// Toplam teminat tutarı
        /// </summary>
        public decimal TotalCoverageAmount { get; set; }

        /// <summary>
        /// Ortalama prim tutarı
        /// </summary>
        public decimal AveragePremiumAmount { get; set; }

        /// <summary>
        /// Ortalama teminat tutarı
        /// </summary>
        public decimal AverageCoverageAmount { get; set; }

        /// <summary>
        /// En yüksek prim tutarı
        /// </summary>
        public decimal MaxPremiumAmount { get; set; }

        /// <summary>
        /// En düşük prim tutarı
        /// </summary>
        public decimal MinPremiumAmount { get; set; }

        /// <summary>
        /// Poliçe türü bazında istatistikler
        /// </summary>
        public List<PolicyTypeStatistics> PolicyTypeStats { get; set; } = new();

        /// <summary>
        /// Sigorta şirketi bazında istatistikler
        /// </summary>
        public List<InsuranceCompanyStatistics> CompanyStats { get; set; } = new();

        /// <summary>
        /// Aylık yenileme takvimi
        /// </summary>
        public List<MonthlyRenewalStatistics> MonthlyRenewals { get; set; } = new();

        /// <summary>
        /// Acil durum poliçeleri
        /// </summary>
        public List<UrgentPolicyAlert> UrgentPolicies { get; set; } = new();

        /// <summary>
        /// Maliyet analizi
        /// </summary>
        public InsuranceCostAnalysis CostAnalysis { get; set; } = new();

        /// <summary>
        /// Aktif poliçe yüzdesi
        /// </summary>
        public decimal ActivePolicyPercentage => TotalPolicies > 0 ?
            (decimal)ActivePolicies / TotalPolicies * 100 : 0;

        /// <summary>
        /// Süresi dolmuş poliçe yüzdesi
        /// </summary>
        public decimal ExpiredPolicyPercentage => TotalPolicies > 0 ?
            (decimal)ExpiredPolicies / TotalPolicies * 100 : 0;

        /// <summary>
        /// Yakında dolacak poliçe yüzdesi
        /// </summary>
        public decimal ExpiringSoonPercentage => TotalPolicies > 0 ?
            (decimal)ExpiringSoonPolicies / TotalPolicies * 100 : 0;

        /// <summary>
        /// Ortalama teminat/prim oranı
        /// </summary>
        public decimal AverageCoveragePremiumRatio => TotalPremiumAmount > 0 ?
            TotalCoverageAmount / TotalPremiumAmount : 0;

        /// <summary>
        /// Rapor özeti
        /// </summary>
        public string Summary =>
            $"{TotalPolicies} poliçe | {ActivePolicies} aktif | " +
            $"{ExpiredPolicies} süresi dolmuş | {ExpiringSoonPolicies} yakında dolacak";

        /// <summary>
        /// Maliyet özeti
        /// </summary>
        public string CostSummary =>
            $"Toplam Prim: {TotalPremiumAmount:N2} ₺ | " +
            $"Toplam Teminat: {TotalCoverageAmount:N2} ₺ | " +
            $"Ortalama: {AveragePremiumAmount:N2} ₺";
    }

    /// <summary>
    /// Poliçe türü istatistikleri
    /// </summary>
    public class PolicyTypeStatistics
    {
        public string PolicyType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal TotalCoverage { get; set; }
        public decimal AveragePremium { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Sigorta şirketi istatistikleri
    /// </summary>
    public class InsuranceCompanyStatistics
    {
        public string CompanyName { get; set; } = string.Empty;
        public int PolicyCount { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal AveragePremium { get; set; }
        public decimal MarketShare { get; set; }
        public int ActivePolicies { get; set; }
        public int ExpiredPolicies { get; set; }
    }

    /// <summary>
    /// Aylık yenileme istatistikleri
    /// </summary>
    public class MonthlyRenewalStatistics
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int RenewalCount { get; set; }
        public decimal RenewalAmount { get; set; }
        public List<string> VehiclePlates { get; set; } = new();
    }

    /// <summary>
    /// Acil durum poliçe uyarısı
    /// </summary>
    public class UrgentPolicyAlert
    {
        public int PolicyId { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string PolicyType { get; set; } = string.Empty;
        public string InsuranceCompany { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string UrgencyLevel { get; set; } = string.Empty;
        public decimal PremiumAmount { get; set; }
    }

    /// <summary>
    /// Sigorta maliyet analizi
    /// </summary>
    public class InsuranceCostAnalysis
    {
        public decimal MonthlyAverageCost { get; set; }
        public decimal YearlyEstimatedCost { get; set; }
        public decimal CostPerVehicle { get; set; }
        public decimal KaskoCost { get; set; }
        public decimal TrafficInsuranceCost { get; set; }
        public decimal OtherInsuranceCost { get; set; }
        public decimal CostIncrease { get; set; }
        public string CostTrend { get; set; } = string.Empty;
    }
}