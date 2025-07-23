using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    /// <summary>
    /// Araç güncelleme için DTO sınıfı
    /// </summary>

    public class UpdateVehicleDto
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string VinNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Color { get; set; }
        public string Type { get; set; } = string.Empty;
        public string FuelType { get; set; } = string.Empty;
        public decimal CurrentKm { get; set; }
        public decimal FuelCapacity { get; set; }
        public decimal EngineSize { get; set; }
        public string? Notes { get; set; }
    }

     

    /// <summary>
    /// Lisans güncelleme için DTO
    /// </summary>
    public class UpdateVehicleLicenseDto
    {
        public int? Id { get; set; } // null ise yeni kayıt

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

        public bool IsActive { get; set; } = true;

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        public string? Notes { get; set; }

        public bool IsDeleted { get; set; } = false; // Silinmek üzere işaretlenen kayıtlar
    }

    /// <summary>
    /// Araç durumu güncelleme için özel DTO
    /// </summary>
    public class UpdateVehicleStatusDto
    {
        [Required(ErrorMessage = "ID zorunludur")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Durum zorunludur")]
        public VehicleStatus Status { get; set; }

        [StringLength(500, ErrorMessage = "Durum açıklaması en fazla 500 karakter olabilir")]
        public string? StatusNote { get; set; }

        public DateTime StatusChangeDate { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Araç kilometre güncelleme için özel DTO
    /// </summary>
    public class UpdateVehicleKilometerDto
    {
        [Required(ErrorMessage = "ID zorunludur")]
        public int Id { get; set; }

        [Range(0, 999999, ErrorMessage = "Geçerli bir kilometre değeri giriniz")]
        public decimal NewKilometer { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Note { get; set; }

        public DateTime RecordDate { get; set; } = DateTime.UtcNow;
    }
}