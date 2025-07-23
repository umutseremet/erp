using ERP.Application.DTOs.Maintenance;

namespace ERP.Application.DTOs.Dashboard
{
    public class MaintenanceDashboardStats
    {
        public int TotalMaintenances { get; set; }
        public int CompletedMaintenances { get; set; }
        public int PendingMaintenances { get; set; } 
        public int ScheduledMaintenances { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public decimal AverageMaintenanceCost { get; set; }
        public decimal MonthlyMaintenanceCost { get; set; }
        public int AverageCompletionTime { get; set; } // days
        public Dictionary<string, int> MaintenancesByType { get; set; } = new();
        public Dictionary<string, decimal> CostByServiceProvider { get; set; } = new();
        public List<MaintenanceItemDto> UpcomingMaintenances { get; set; } = new();
        public List<MaintenanceItemDto> OverdueMaintenances { get; set; } = new();
    }
}