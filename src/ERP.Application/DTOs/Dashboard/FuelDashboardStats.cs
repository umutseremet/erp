namespace ERP.Application.DTOs.Dashboard
{
    public class FuelDashboardStats
    {
        public decimal TotalFuelCost { get; set; }
        public decimal MonthlyFuelCost { get; set; }
        public decimal TotalFuelQuantity { get; set; }
        public decimal MonthlyFuelQuantity { get; set; }
        public decimal AverageFuelConsumption { get; set; } // L/100km
        public decimal AverageFuelPrice { get; set; }
        public int TotalTransactions { get; set; }
        public int MonthlyTransactions { get; set; }
        public Dictionary<string, decimal> CostByFuelType { get; set; } = new();
        public Dictionary<string, decimal> ConsumptionByVehicleType { get; set; } = new();
        public Dictionary<string, decimal> CostByMonth { get; set; } = new();
        public List<VehicleFuelEfficiencyDto> TopEfficientVehicles { get; set; } = new();
        public List<VehicleFuelEfficiencyDto> LeastEfficientVehicles { get; set; } = new();
    }

    public class VehicleFuelEfficiencyDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal FuelEfficiency { get; set; } // L/100km
        public decimal MonthlyFuelCost { get; set; }
    }
}