using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Maintenance
{
    /// <summary>
    /// Bakım güncelleme için DTO
    /// </summary>
    public class UpdateMaintenanceDto
    {
        /// <summary>
        /// Bakım ID'si
        /// </summary>
        [Required(ErrorMessage = "Bakım ID'si gereklidir")]
        public int Id { get; set; }

        /// <summary>
        /// Bakım türü
        /// </summary>
        [Required(ErrorMessage = "Bakım türü seçimi gereklidir")]
        public MaintenanceType Type { get; set; }

        /// <summary>
        /// Planlanan tarih
        /// </summary>
        [Required(ErrorMessage = "Planlanan tarih gereklidir")]
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Bakım açıklaması
        /// </summary>
        [Required(ErrorMessage = "Bakım açıklaması gereklidir")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Açıklama 5-200 karakter arasında olmalıdır")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Servis sağlayıcısı
        /// </summary>
        [StringLength(200, ErrorMessage = "Servis sağlayıcısı en fazla 200 karakter olabilir")]
        public string? ServiceProvider { get; set; }

        /// <summary>
        /// Tahmini/Gerçek maliyet
        /// </summary>
        [Range(0, 100000, ErrorMessage = "Maliyet 0-100.000 arasında olmalıdır")]
        public decimal? Cost { get; set; }

        /// <summary>
        /// Sonraki bakım tarihi
        /// </summary>
        public DateTime? NextMaintenanceDate { get; set; }

        /// <summary>
        /// Sonraki bakım kilometresi
        /// </summary>
        [Range(0, 9999999, ErrorMessage = "Sonraki bakım kilometresi 0-9999999 arasında olmalıdır")]
        public decimal? NextMaintenanceKm { get; set; }

        /// <summary>
        /// Notlar
        /// </summary>
        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        public string? Notes { get; set; }

        /// <summary>
        /// Güncelleme sebebi
        /// </summary>
        [StringLength(500, ErrorMessage = "Güncelleme sebebi en fazla 500 karakter olabilir")]
        public string? UpdateReason { get; set; }

        /// <summary>
        /// Yeniden planlandı mı?
        /// </summary>
        public bool IsRescheduled { get; set; } = false;
    }
}