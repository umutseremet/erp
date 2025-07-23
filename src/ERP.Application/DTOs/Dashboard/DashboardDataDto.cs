using ERP.Application.DTOs.Notification;

namespace ERP.Application.DTOs.Dashboard
{
    public class DashboardDataDto
    {
        public VehicleDashboardStats VehicleStats { get; set; } = new();
        public MaintenanceDashboardStats MaintenanceStats { get; set; } = new();
        public FuelDashboardStats FuelStats { get; set; } = new();
        public InsuranceDashboardStats InsuranceStats { get; set; } = new();
        public List<NotificationSummaryDto> RecentNotifications { get; set; } = new();
        public List<AlertDto> CriticalAlerts { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class InsuranceDashboardStats
    {
        public int TotalPolicies { get; set; }
        public int ActivePolicies { get; set; }
        public int ExpiringPolicies { get; set; }
        public int ExpiredPolicies { get; set; }
        public decimal TotalPremiumAmount { get; set; }
        public decimal MonthlyPremiumAmount { get; set; }
        public Dictionary<string, int> PoliciesByCompany { get; set; } = new();
        public Dictionary<string, int> PoliciesByType { get; set; } = new();
    }

    public class AlertDto
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? ActionUrl { get; set; }
    }
}