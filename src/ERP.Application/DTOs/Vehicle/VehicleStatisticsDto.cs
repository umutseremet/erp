namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleStatisticsDto
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int AssignedVehicles { get; set; }
        public int InMaintenanceVehicles { get; set; }
        public decimal AverageAge { get; set; }
        public decimal TotalKilometers { get; set; }
        public decimal AverageKilometers { get; set; }
        public Dictionary<string, int> VehiclesByBrand { get; set; } = new();
        public Dictionary<string, int> VehiclesByType { get; set; } = new();
    }
}
