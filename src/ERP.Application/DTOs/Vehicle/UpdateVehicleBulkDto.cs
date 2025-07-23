using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    public class UpdateVehicleBulkDto
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public VehicleStatus Status { get; set; }
        public decimal CurrentKm { get; set; }
    }
}
