using ERP.Application.DTOs.Common;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleDto : BaseDto
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
        public bool HasActiveInsurance { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
    }
}