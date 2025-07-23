using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    /// <summary>
    /// Yeni araç oluşturma için DTO sınıfı
    /// </summary>
    /// 
    public class CreateVehicleDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string VinNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Color { get; set; }
        public string Type { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public decimal FuelCapacity { get; set; }
        public decimal EngineSize { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentKm { get; set; }
        public string? Notes { get; set; }
    }

     
    /// <summary>
    /// Araç oluşturma sırasında lisans bilgileri için DTO
    /// </summary>
    public class CreateVehicleLicenseDto
    {
        [Required(ErrorMessage = "Lisans numarası zorunludur")]
        [StringLength(100, ErrorMessage = "Lisans numarası en fazla 100 karakter olabilir")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lisans türü zorunludur")]
        [StringLength(50, ErrorMessage = "Lisans türü en fazla 50 karakter olabilir")]
        public string LicenseType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Düzenleme tarihi zorunludur")]
        public DateTime IssueDate { get; set; }

        [Required(ErrorMessage = "Bitiş tarihi zorunludur")]
        public DateTime ExpiryDate { get; set; }

        [StringLength(200, ErrorMessage = "Düzenleyen otorite en fazla 200 karakter olabilir")]
        public string? IssuingAuthority { get; set; }

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        public string? Notes { get; set; }
    }
}