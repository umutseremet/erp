// VehiclePerformanceDto.cs
namespace ERP.Application.DTOs.Vehicle
{
    public class VehiclePerformanceDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public decimal Reliability { get; set; }
        public decimal MaintenanceCostRatio { get; set; }
        public int BreakdownCount { get; set; }
        public decimal AvailabilityRate { get; set; }
        public decimal UtilizationRate { get; set; }
        public decimal FuelEfficiency { get; set; }
        public decimal CostPerKilometer { get; set; }
        public int MaintenanceCompliance { get; set; }
        public string PerformanceGrade { get; set; } = string.Empty; // A, B, C, D, F
        public DateTime AnalysisPeriodStart { get; set; }
        public DateTime AnalysisPeriodEnd { get; set; }
        public List<PerformanceMetricDto> Metrics { get; set; } = new List<PerformanceMetricDto>();
        public List<string> Recommendations { get; set; } = new List<string>();
    }

    public class PerformanceMetricDto
    {
        public string MetricName { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal Benchmark { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Good, Warning, Critical
    }
}