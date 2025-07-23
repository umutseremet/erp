namespace ERP.Application.DTOs.Maintenance
{
    public class MaintenancePerformanceReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalMaintenances { get; set; }
        public int CompletedMaintenances { get; set; }
        public int PendingMaintenances { get; set; }
        public int OverdueMaintenances { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public double AverageCompletionTime { get; set; } // days
        public double OnTimeCompletionRate { get; set; } // percentage
        public List<MaintenanceByTypeDto> MaintenancesByType { get; set; } = new();
        public List<ServiceProviderPerformanceDto> ServiceProviderPerformance { get; set; } = new();
        public List<VehicleMaintenanceStatsDto> VehicleMaintenanceStats { get; set; } = new();
        public List<MonthlyMaintenanceTrendDto> MonthlyTrends { get; set; } = new();
    }

    public class MaintenanceByTypeDto
    {
        public string MaintenanceType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public double AverageCompletionTime { get; set; }
    }

    public class VehicleMaintenanceStatsDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int MaintenanceCount { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCost { get; set; }
        public double AverageCompletionTime { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }

    public class MonthlyMaintenanceTrendDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public int MaintenanceCount { get; set; }
        public decimal TotalCost { get; set; }
        public double AverageCompletionTime { get; set; }
    }
}