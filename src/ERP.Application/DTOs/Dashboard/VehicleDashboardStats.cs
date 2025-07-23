namespace ERP.Application.DTOs.Dashboard
{
    public class VehicleDashboardStats
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int AssignedVehicles { get; set; }
        public int InMaintenanceVehicles { get; set; }
        public int OutOfServiceVehicles { get; set; }
        public int InspectionDueVehicles { get; set; }
        public decimal AverageAge { get; set; }
        public decimal TotalKilometers { get; set; }
        public decimal AverageKilometers { get; set; }
        public Dictionary<string, int> VehiclesByBrand { get; set; } = new();
        public Dictionary<string, int> VehiclesByType { get; set; } = new();
        public Dictionary<string, int> VehiclesByStatus { get; set; } = new();
    }
}