namespace ERP.Application.DTOs.FuelTransaction
{
    public class FuelEfficiencyDto
    {
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public decimal AverageConsumption { get; set; } // L/100km
        public decimal TotalFuelUsed { get; set; }
        public decimal TotalKilometers { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageCostPerKm { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
