// DTOs/Maintenance/MaintenanceSupportClasses.cs - Missing Support Classes
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Maintenance
{
    /// <summary>
    /// Bakım türü istatistikleri
    /// </summary>
    public class MaintenanceTypeStatistics
    {
        public MaintenanceType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public int Count { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public int OverdueCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public decimal MinCost { get; set; }
        public decimal MaxCost { get; set; }
        public decimal Percentage { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageCompletionTime { get; set; } // gün
        public int Priority { get; set; }

        public string Summary => $"{TypeDisplayName}: {Count} bakım, {CompletionRate:F1}% tamamlandı";
        public bool IsHighVolume => Count > 20;
        public bool IsHighCost => AverageCost > 2000;
    }

    /// <summary>
    /// Araç bakım istatistikleri
    /// </summary>
    public class VehicleMaintenanceStatistics
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public int VehicleYear { get; set; }
        public decimal CurrentKm { get; set; }
        public int MaintenanceCount { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public int OverdueCount { get; set; }
        public int EmergencyCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public decimal CostPerKm { get; set; }
        public DateTime LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public int MaintenanceFrequency { get; set; } // ortalama gün olarak
        public decimal DowntimeHours { get; set; }
        public decimal ReliabilityScore { get; set; }

        public string VehicleInfo => $"{VehicleYear} {VehicleBrand} {VehicleModel}";
        public string FullInfo => $"{PlateNumber} - {VehicleInfo}";
        public bool IsHighMaintenance => MaintenanceCount > 15;
        public bool IsProblematic => OverdueCount > 3 || EmergencyCount > 2;
        public bool IsReliable => ReliabilityScore > 80;
        public decimal AvailabilityRate => 100 - (DowntimeHours / (24 * 365) * 100);
    }

    /// <summary>
    /// Servis sağlayıcısı istatistikleri
    /// </summary>
    public class ServiceProviderStatistics
    {
        public string ProviderName { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int MaintenanceCount { get; set; }
        public int CompletedCount { get; set; }
        public int OnTimeCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public decimal MinCost { get; set; }
        public decimal MaxCost { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal OnTimeRate { get; set; }
        public decimal AverageCompletionTime { get; set; } // saat
        public decimal CustomerSatisfaction { get; set; } // 1-10 arası
        public decimal MarketShare { get; set; }
        public int RepeatCustomerRate { get; set; }
        public decimal QualityScore { get; set; }
        public List<string> Specializations { get; set; } = new();

        public decimal PerformanceScore => (CompletionRate + OnTimeRate + (CustomerSatisfaction * 10) + QualityScore) / 4;
        public bool IsPreferred => PerformanceScore > 85;
        public bool IsReliable => OnTimeRate > 90 && CompletionRate > 95;
        public string Rating => PerformanceScore switch
        {
            >= 90 => "Mükemmel",
            >= 80 => "İyi",
            >= 70 => "Orta",
            >= 60 => "Zayıf",
            _ => "Yetersiz"
        };
    }

    /// <summary>
    /// Aylık bakım istatistikleri
    /// </summary>
    public class MonthlyMaintenanceStatistics
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public string Period => $"{MonthName} {Year}";
        public int MaintenanceCount { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public int OverdueCount { get; set; }
        public int EmergencyCount { get; set; }
        public int RoutineCount { get; set; }
        public int PreventiveCount { get; set; }
        public int CorrectiveCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public decimal BudgetedCost { get; set; }
        public decimal BudgetVariance => TotalCost - BudgetedCost;
        public decimal BudgetUtilization => BudgetedCost > 0 ? (TotalCost / BudgetedCost * 100) : 0;
        public int WorkingDays { get; set; }
        public decimal DailyAverage => WorkingDays > 0 ? (decimal)MaintenanceCount / WorkingDays : 0;
        public decimal CostPerDay => WorkingDays > 0 ? TotalCost / WorkingDays : 0;

        public decimal CompletionRate => MaintenanceCount > 0 ? (decimal)CompletedCount / MaintenanceCount * 100 : 0;
        public decimal EmergencyRate => MaintenanceCount > 0 ? (decimal)EmergencyCount / MaintenanceCount * 100 : 0;
        public decimal PlannedMaintenanceRate => MaintenanceCount > 0 ? (decimal)(RoutineCount + PreventiveCount) / MaintenanceCount * 100 : 0;
        public bool IsOverBudget => BudgetVariance > 0;
        public bool IsHighActivity => MaintenanceCount > 50;
        public string Performance => CompletionRate > 90 ? "İyi" : CompletionRate > 80 ? "Orta" : "Zayıf";
    }

    /// <summary>
    /// Haftalık bakım istatistikleri
    /// </summary>
    public class WeeklyMaintenanceStatistics
    {
        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public DateTime WeekStartDate { get; set; }
        public DateTime WeekEndDate { get; set; }
        public string WeekPeriod => $"Hafta {WeekNumber}, {Year} ({WeekStartDate:dd.MM} - {WeekEndDate:dd.MM})";
        public int MaintenanceCount { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public int OverdueCount { get; set; }
        public int EmergencyCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public int WorkingDays { get; set; } = 5;
        public decimal DailyAverage => WorkingDays > 0 ? (decimal)MaintenanceCount / WorkingDays : 0;
        public decimal CostPerDay => WorkingDays > 0 ? TotalCost / WorkingDays : 0;

        // Günlük dağılım
        public int MondayCount { get; set; }
        public int TuesdayCount { get; set; }
        public int WednesdayCount { get; set; }
        public int ThursdayCount { get; set; }
        public int FridayCount { get; set; }
        public int SaturdayCount { get; set; }
        public int SundayCount { get; set; }

        public decimal CompletionRate => MaintenanceCount > 0 ? (decimal)CompletedCount / MaintenanceCount * 100 : 0;
        public decimal EmergencyRate => MaintenanceCount > 0 ? (decimal)EmergencyCount / MaintenanceCount * 100 : 0;
        public bool IsHighActivity => MaintenanceCount > 15;
        public bool IsBusyWeek => DailyAverage > 3;
        public string MostBusyDay => GetMostBusyDay();

        private string GetMostBusyDay()
        {
            var days = new Dictionary<string, int>
            {
                ["Pazartesi"] = MondayCount,
                ["Salı"] = TuesdayCount,
                ["Çarşamba"] = WednesdayCount,
                ["Perşembe"] = ThursdayCount,
                ["Cuma"] = FridayCount,
                ["Cumartesi"] = SaturdayCount,
                ["Pazar"] = SundayCount
            };
            return days.OrderByDescending(d => d.Value).First().Key;
        }
    }

    /// <summary>
    /// Vade geçmiş bakım uyarısı
    /// </summary>
    public class OverdueMaintenanceAlert
    {
        public int MaintenanceId { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public MaintenanceType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public int DelayDays { get; set; }
        public string ServiceProvider { get; set; } = string.Empty;
        public decimal? EstimatedCost { get; set; }
        public string UrgencyLevel { get; set; } = string.Empty;
        public string DelayReason { get; set; } = string.Empty;
        public string AssignedUser { get; set; } = string.Empty;
        public DateTime LastContactDate { get; set; }
        public string Status { get; set; } = string.Empty;

        public string AlertMessage => $"🔴 {VehiclePlateNumber} - {TypeDisplayName} - {DelayDays} gün gecikmiş";
        public string UrgencyIndicator => DelayDays > 30 ? "🚨 KRİTİK" : DelayDays > 7 ? "⚠️ YÜKSEK" : "🟡 ORTA";
        public string VehicleInfo => $"{VehiclePlateNumber} ({VehicleBrand} {VehicleModel})";
        public bool IsCritical => DelayDays > 30 || Type == MaintenanceType.Emergency;
        public bool NeedsImmediateAction => DelayDays > 14;
        public string ActionRequired => IsCritical ? "Acil Müdahale" : NeedsImmediateAction ? "Hızlı Çözüm" : "Planlama";
    }

    /// <summary>
    /// Yaklaşan bakım uyarısı
    /// </summary>
    public class UpcomingMaintenanceAlert
    {
        public int MaintenanceId { get; set; }
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public string VehicleModel { get; set; } = string.Empty;
        public MaintenanceType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public int DaysUntilDue { get; set; }
        public string ServiceProvider { get; set; } = string.Empty;
        public decimal? EstimatedCost { get; set; }
        public bool IsScheduled { get; set; }
        public bool IsConfirmed { get; set; }
        public string AssignedUser { get; set; } = string.Empty;
        public string PreparationStatus { get; set; } = string.Empty;
        public List<string> RequiredParts { get; set; } = new();

        public string AlertMessage => $"📅 {VehiclePlateNumber} - {TypeDisplayName} - {DaysUntilDue} gün sonra";
        public string PriorityIndicator => DaysUntilDue <= 1 ? "🔴 BUGÜN" : DaysUntilDue <= 3 ? "🟡 YAKIN" : "🟢 NORMAL";
        public string VehicleInfo => $"{VehiclePlateNumber} ({VehicleBrand} {VehicleModel})";
        public bool IsToday => DaysUntilDue == 0;
        public bool IsThisWeek => DaysUntilDue <= 7;
        public bool RequiresPreparation => RequiredParts.Any() || !IsScheduled;
        public string ReadinessStatus => (IsScheduled && IsConfirmed) ? "Hazır" : RequiresPreparation ? "Hazırlık Gerekli" : "Beklemede";
    }

    /// <summary>
    /// Bakım maliyet analizi
    /// </summary>
    public class MaintenanceCostAnalysis
    {
        public decimal MonthlyAverageCost { get; set; }
        public decimal YearlyEstimatedCost { get; set; }
        public decimal CostPerVehicle { get; set; }
        public decimal CostPerKilometer { get; set; }
        public decimal PreventiveCost { get; set; }
        public decimal CorrectiveCost { get; set; }
        public decimal EmergencyCost { get; set; }
        public decimal RoutineCost { get; set; }
        public decimal BudgetedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal BudgetVariance => ActualAmount - BudgetedAmount;
        public decimal BudgetUtilizationRate => BudgetedAmount > 0 ? (ActualAmount / BudgetedAmount * 100) : 0;
        public decimal CostIncreaseTrend { get; set; }
        public string MostExpensiveType { get; set; } = string.Empty;
        public string CostTrend { get; set; } = string.Empty;
        public List<CostSavingRecommendation> Recommendations { get; set; } = new();
        public Dictionary<string, decimal> CostDistribution { get; set; } = new();
        public Dictionary<string, decimal> MonthlyTrend { get; set; } = new();

        public decimal PreventiveToCorrectiveRatio => CorrectiveCost > 0 ? PreventiveCost / CorrectiveCost : 0;
        public decimal EmergencyRatio => ActualAmount > 0 ? EmergencyCost / ActualAmount * 100 : 0;
        public bool IsOverBudget => BudgetVariance > 0;
        public bool HasCostIncrease => CostIncreaseTrend > 0;
        public string BudgetStatus => IsOverBudget ? "Aşım" : "Uygun";
        public string CostEfficiency => PreventiveToCorrectiveRatio > 2 ? "İyi" : PreventiveToCorrectiveRatio > 1 ? "Orta" : "Zayıf";
    }

    /// <summary>
    /// Maliyet tasarrufu önerisi
    /// </summary>
    public class CostSavingRecommendation
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedSavings { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string Implementation { get; set; } = string.Empty;
        public int ImplementationTimeWeeks { get; set; }
        public decimal ImplementationCost { get; set; }
        public decimal ROI => ImplementationCost > 0 ? EstimatedSavings / ImplementationCost : 0;
        public string Difficulty { get; set; } = string.Empty;
        public List<string> RequiredResources { get; set; } = new();
        public List<string> Risks { get; set; } = new();

        public string PriorityLevel => Priority switch
        {
            1 => "Yüksek",
            2 => "Orta",
            3 => "Düşük",
            _ => "Belirsiz"
        };
        public bool IsQuickWin => ImplementationTimeWeeks <= 4 && EstimatedSavings > 1000;
        public bool IsHighImpact => EstimatedSavings > 10000;
        public string ImpactLevel => IsHighImpact ? "Yüksek" : EstimatedSavings > 5000 ? "Orta" : "Düşük";
    }

    /// <summary>
    /// Bakım performans metrikleri
    /// </summary>
    public class MaintenancePerformanceMetrics
    {
        public decimal AverageResponseTime { get; set; } // saat
        public decimal AverageRepairTime { get; set; } // saat
        public decimal VehicleAvailabilityRate { get; set; } // %
        public decimal FirstTimeFixRate { get; set; } // %
        public decimal PlannedMaintenanceRate { get; set; } // %
        public decimal EmergencyMaintenanceRate { get; set; } // %
        public decimal RecurringFailureRate { get; set; } // %
        public decimal ServiceQualityScore { get; set; } // 1-10
        public decimal DowntimeReduction { get; set; } // %
        public decimal PreventiveReactiveRatio { get; set; }
        public decimal MaintenanceEfficiency { get; set; } // %
        public decimal CostEffectiveness { get; set; } // %
        public decimal ScheduleAdherence { get; set; } // %
        public decimal TechnicianUtilization { get; set; } // %

        public string PerformanceSummary
        {
            get
            {
                var score = (VehicleAvailabilityRate + FirstTimeFixRate + PlannedMaintenanceRate + ServiceQualityScore * 10) / 4;
                return score switch
                {
                    >= 90 => "Mükemmel",
                    >= 80 => "İyi",
                    >= 70 => "Orta",
                    >= 60 => "Zayıf",
                    _ => "Kritik"
                };
            }
        }

        public List<PerformanceIndicator> Indicators { get; set; } = new();
        public List<ImprovementRecommendation> ImprovementRecommendations { get; set; } = new();

        public decimal OverallEfficiencyScore => (MaintenanceEfficiency + CostEffectiveness + ScheduleAdherence) / 3;
        public bool MeetsStandards => VehicleAvailabilityRate > 95 && FirstTimeFixRate > 85;
        public bool NeedsImprovement => EmergencyMaintenanceRate > 25 || RecurringFailureRate > 15;
    }

    /// <summary>
    /// Performans göstergesi
    /// </summary>
    public class PerformanceIndicator
    {
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal Target { get; set; }
        public decimal Variance => Value - Target;
        public string Status { get; set; } = string.Empty;
        public string Trend { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; }

        public decimal PerformancePercentage => Target > 0 ? (Value / Target * 100) : 0;
        public bool IsOnTarget => Math.Abs(Variance) <= (Target * 0.05m); // 5% tolerance
        public bool ExceedsTarget => Value > Target;
        public bool BelowTarget => Value < Target;
        public string StatusIndicator => IsOnTarget ? "✅" : ExceedsTarget ? "📈" : "📉";
    }

    /// <summary>
    /// İyileştirme önerisi
    /// </summary>
    public class ImprovementRecommendation
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Priority { get; set; }
        public decimal Impact { get; set; }
        public string Implementation { get; set; } = string.Empty;
        public string ExpectedBenefit { get; set; } = string.Empty;
        public int EstimatedDuration { get; set; } // hafta
        public decimal EstimatedCost { get; set; }
        public List<string> RequiredSkills { get; set; } = new();
        public List<string> Dependencies { get; set; } = new();
        public string Difficulty { get; set; } = string.Empty;
        public decimal SuccessProbability { get; set; } // %

        public string PriorityLevel => Priority switch
        {
            1 => "Kritik",
            2 => "Yüksek",
            3 => "Orta",
            4 => "Düşük",
            _ => "Belirsiz"
        };

        public string ImpactLevel => Impact switch
        {
            >= 80 => "Çok Yüksek",
            >= 60 => "Yüksek",
            >= 40 => "Orta",
            >= 20 => "Düşük",
            _ => "Çok Düşük"
        };

        public bool IsQuickWin => EstimatedDuration <= 2 && Impact >= 60;
        public bool IsLowRisk => SuccessProbability >= 80 && EstimatedCost <= 5000;
        public decimal ROI => EstimatedCost > 0 ? Impact / EstimatedCost * 1000 : 0; // Simplified ROI
    }
}