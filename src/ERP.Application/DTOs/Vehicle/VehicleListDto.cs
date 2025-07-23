using ERP.Application.DTOs.Common;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    /// <summary>
    /// Araç listesi görünümü için DTO sınıfı
    /// </summary> 
    public class VehicleListDto : BaseDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal CurrentKm { get; set; }
        public string? AssignedUserName { get; set; }
        public bool HasActiveInsurance { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? Notes { get; set; }
    }
    /// <summary>
    /// Araç uyarıları için DTO sınıfı
    /// </summary>
    public class VehicleWarning
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public VehicleWarning() { }

        public VehicleWarning(string type, string message, string color, string icon)
        {
            Type = type;
            Message = message;
            Color = color;
            Icon = icon;
        }
    }
}