using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.FuelTransaction;
using ERP.Application.DTOs.Insurance;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    /// <summary>
    /// Araç detay görünümü için DTO sınıfı
    /// </summary>

    public class VehicleDetailDto : BaseDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string VinNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Color { get; set; }
        public string Type { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal CurrentKm { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal EngineSize { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public int? AssignedUserId { get; set; }
        public string? AssignedUserName { get; set; }
        public string? Notes { get; set; }
        public List<VehicleMaintenanceDto>? MaintenanceHistory { get; set; }
        public List<InsurancePolicyDto>? InsurancePolicies { get; set; }
        public List<FuelTransactionDto>? FuelTransactions { get; set; }
        public List<VehicleInspectionDto> Inspections { get; internal set; }
        public List<VehicleLicenseDto> Licenses { get; internal set; }
        public List<FuelTransactionSummaryDto> RecentFuelTransactions { get; internal set; }
    }

    public class VehicleAssignedUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime AssignedDate { get; set; }
        public string FormattedAssignedDate => AssignedDate.ToString("dd.MM.yyyy");
    }

    public class VehicleInsuranceDto
    {
        public int Id { get; set; }
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
        public bool IsExpired => DateTime.UtcNow > EndDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > EndDate;
        public int DaysUntilExpiry => (EndDate - DateTime.UtcNow).Days;
        public string FormattedPremium => $"{PremiumAmount:C} {Currency}";
        public string FormattedCoverage => $"{CoverageAmount:C} {Currency}";
        public string ValidityPeriod => $"{StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}";
    }

    public class VehicleLicenseDto
    {
        public int Id { get; set; }
        public string LicenseNumber { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? IssuingAuthority { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > ExpiryDate;
        public int DaysUntilExpiry => (ExpiryDate - DateTime.UtcNow).Days;
        public string ValidityPeriod => $"{IssueDate:dd.MM.yyyy} - {ExpiryDate:dd.MM.yyyy}";
    }

    public class VehicleMaintenanceDto
    {
        public int Id { get; set; }
        public MaintenanceType Type { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal VehicleKm { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ServiceProvider { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public string? Notes { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsOverdue => !IsCompleted && DateTime.UtcNow > ScheduledDate;
        public string TypeText => Type switch
        {
            MaintenanceType.Routine => "Rutin",
            MaintenanceType.Preventive => "Önleyici",
            MaintenanceType.Corrective => "Düzeltici",
            MaintenanceType.Emergency => "Acil",
            MaintenanceType.Inspection => "Muayene",
            MaintenanceType.Overhaul => "Kapsamlı",
            _ => Type.ToString()
        };
        public string StatusText => IsCompleted ? "Tamamlandı" : (IsOverdue ? "Gecikmiş" : "Planlandı");
        public string FormattedCost => Cost?.ToString("C") ?? "Belirtilmemiş";
    }

    public class VehicleInspectionDto
    {
        public int Id { get; set; }
        public DateTime InspectionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public InspectionStatus Status { get; set; }
        public string? InspectionCenter { get; set; }
        public string? CertificateNumber { get; set; }
        public decimal VehicleKm { get; set; }
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > ExpiryDate;
        public string StatusText => Status switch
        {
            InspectionStatus.Pending => "Beklemede",
            InspectionStatus.Scheduled => "Planlandı",
            InspectionStatus.InProgress => "Devam Ediyor",
            InspectionStatus.Passed => "Geçti",
            InspectionStatus.Failed => "Başarısız",
            InspectionStatus.Expired => "Süresi Doldu",
            InspectionStatus.Cancelled => "İptal Edildi",
            _ => Status.ToString()
        };
    }

    public class VehicleFuelCardDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public string FormattedBalance => $"{CurrentBalance:C}";
        public string FormattedLimit => $"{CreditLimit:C}";
        public decimal UsagePercentage => CreditLimit > 0 ? ((CreditLimit - CurrentBalance) / CreditLimit) * 100 : 0;
    }

    public class FuelTransactionSummaryDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public FuelType FuelType { get; set; }
        public string? StationName { get; set; }
        public decimal VehicleKm { get; set; }
        public string FormattedAmount => $"{TotalAmount:C}";
        public string FormattedQuantity => $"{Quantity:F2} L";
        public string FormattedDate => TransactionDate.ToString("dd.MM.yyyy");
    }

    public class VehicleFuelStatisticsDto
    {
        public decimal TotalFuelCost { get; set; }
        public decimal TotalFuelQuantity { get; set; }
        public decimal AverageConsumption { get; set; } // L/100km
        public decimal AverageFuelPrice { get; set; }
        public int TransactionCount { get; set; }
        public DateTime? LastFuelDate { get; set; }
        public string FormattedTotalCost => $"{TotalFuelCost:C}";
        public string FormattedConsumption => $"{AverageConsumption:F2} L/100km";
        public string FormattedAveragePrice => $"{AverageFuelPrice:F2} ₺/L";
    }

    public class VehicleLocationDto
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? Address { get; set; }
        public DateTime RecordedAt { get; set; }
        public decimal? Speed { get; set; }
        public string? Direction { get; set; }
        public string? DataSource { get; set; }
        public string FormattedSpeed => Speed.HasValue ? $"{Speed:F0} km/h" : "Bilinmiyor";
        public string FormattedDate => RecordedAt.ToString("dd.MM.yyyy HH:mm");
    }

    public class VehicleTireChangeDto
    {
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public decimal VehicleKm { get; set; }
        public string TireBrand { get; set; } = string.Empty;
        public string TireSize { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string? ServiceProvider { get; set; }
        public decimal? Cost { get; set; }
        public string? Notes { get; set; }
        public string FormattedCost => Cost?.ToString("C") ?? "Belirtilmemiş";
        public string TireInfo => $"{TireBrand} {TireSize} ({Quantity} Adet)";
    }

    public class VehicleNotificationDto
    {
        public int Id { get; set; }
        public NotificationType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public int Priority { get; set; }
        public string PriorityText => Priority switch
        {
            1 => "Düşük",
            2 => "Orta",
            3 => "Yüksek",
            4 => "Kritik",
            _ => "Bilinmiyor"
        };
    } 
}