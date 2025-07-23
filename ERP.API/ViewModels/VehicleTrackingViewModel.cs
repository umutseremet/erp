namespace ERP.API.ViewModels
{
    public class VehicleTrackingViewModel
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string VinNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Konum Bilgileri
        public decimal? CurrentLatitude { get; set; }
        public decimal? CurrentLongitude { get; set; }
        public string? CurrentAddress { get; set; }
        public DateTime? LastLocationUpdate { get; set; }

        // Atama Bilgileri
        public string? AssignedUserName { get; set; }
        public string? AssignedUserPhone { get; set; }
        public DateTime? AssignmentDate { get; set; }

        // Bakım Bilgileri
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? MaintenanceStatus { get; set; }

        // Yakıt Bilgileri
        public decimal? CurrentFuelLevel { get; set; }
        public decimal? FuelConsumption { get; set; }
        public decimal? TotalDistance { get; set; }

        // Sigorta ve Muayene Bilgileri
        public DateTime? InsuranceExpiryDate { get; set; }
        public DateTime? InspectionExpiryDate { get; set; }

        // Ruhsat Bilgileri
        public string? RegistrationNumber { get; set; }
        public DateTime? RegistrationExpiryDate { get; set; }
    }
}