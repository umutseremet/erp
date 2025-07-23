namespace ERP.Application.DTOs.Maintenance
{
    public class MaintenanceEfficiencyDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double OverallEfficiencyScore { get; set; } // percentage
        public double CostEfficiency { get; set; } // percentage
        public double TimeEfficiency { get; set; } // percentage
        public double ResourceUtilization { get; set; } // percentage
        public double PreventiveMaintenanceRatio { get; set; } // percentage
        public double UnplannedMaintenanceRatio { get; set; } // percentage
        public decimal CostPerKilometer { get; set; }
        public decimal CostPerVehicle { get; set; }
        public double MeanTimeBetweenFailures { get; set; } // days
        public double MeanTimeToRepair { get; set; } // days
        public List<EfficiencyByVehicleTypeDto> EfficiencyByVehicleType { get; set; } = new();
        public List<EfficiencyTrendDto> EfficiencyTrends { get; set; } = new();
    }

    public class EfficiencyByVehicleTypeDto
    {
        public string VehicleType { get; set; } = string.Empty;
        public double EfficiencyScore { get; set; }
        public decimal AverageCost { get; set; }
        public double AverageTime { get; set; }
        public int VehicleCount { get; set; }
        public int MaintenanceCount { get; set; }
    }

    public class EfficiencyTrendDto
    {
        public DateTime Date { get; set; }
        public double EfficiencyScore { get; set; }
        public decimal TotalCost { get; set; }
        public double AverageTime { get; set; }
        public int MaintenanceCount { get; set; }
    }
}