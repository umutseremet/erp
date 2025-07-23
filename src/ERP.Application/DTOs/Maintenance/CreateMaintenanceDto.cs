using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Maintenance
{
    /// <summary>
    /// Yeni bakım oluşturma için DTO
    /// </summary>
    public class CreateMaintenanceDto
    {
        /// <summary>
        /// Araç ID'si
        /// </summary>
        [Required(ErrorMessage = "Araç seçimi gereklidir")]
        public int VehicleId { get; set; }

        /// <summary>
        /// Bakım türü
        /// </summary>
        [Required(ErrorMessage = "Bakım türü seçimi gereklidir")]
        public MaintenanceType Type { get; set; }

        /// <summary>
        /// Planlanan tarih
        /// </summary>
        [Required(ErrorMessage = "Planlanan tarih gereklidir")]
        public DateTime ScheduledDate { get; set; } = DateTime.Today.AddDays(1);

        /// <summary>
        /// Araç kilometre bilgisi
        /// </summary>
        [Required(ErrorMessage = "Araç kilometresi gereklidir")]
        [Range(0, 9999999, ErrorMessage = "Araç kilometresi 0-9999999 arasında olmalıdır")]
        public decimal VehicleKm { get; set; }

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
        /// Tahmini maliyet
        /// </summary>
        [Range(0, 100000, ErrorMessage = "Tahmini maliyet 0-100.000 arasında olmalıdır")]
        public decimal? EstimatedCost { get; set; }

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
        /// Öncelikli bakım mı?
        /// </summary>
        public bool IsPriority { get; set; } = false;

        /// <summary>
        /// Otomatik hatırlatma gönderilsin mi?
        /// </summary>
        public bool SendReminder { get; set; } = true;

        /// <summary>
        /// Hatırlatma günü (kaç gün önce)
        /// </summary>
        [Range(1, 30, ErrorMessage = "Hatırlatma günü 1-30 arasında olmalıdır")]
        public int ReminderDaysBefore { get; set; } = 3;

        /// <summary>
        /// Açıklama temizlenmiş
        /// </summary>
        public string CleanDescription => Description?.Trim() ?? string.Empty;

        /// <summary>
        /// Servis sağlayıcısı temizlenmiş
        /// </summary>
        public string CleanServiceProvider => ServiceProvider?.Trim() ?? string.Empty;

        /// <summary>
        /// Notlar temizlenmiş
        /// </summary>
        public string CleanNotes => Notes?.Trim() ?? string.Empty;

        /// <summary>
        /// Geçmiş tarihli bakım mı?
        /// </summary>
        public bool IsBackdated => ScheduledDate.Date < DateTime.Today;

        /// <summary>
        /// Uzak gelecekte mi? (6 aydan fazla)
        /// </summary>
        public bool IsFarFuture => ScheduledDate.Date > DateTime.Today.AddMonths(6);

        /// <summary>
        /// Yakın zamanda mı? (7 gün içinde)
        /// </summary>
        public bool IsNearFuture => ScheduledDate.Date <= DateTime.Today.AddDays(7);

        /// <summary>
        /// Acil bakım mı?
        /// </summary>
        public bool IsEmergency => Type == MaintenanceType.Emergency;

        /// <summary>
        /// Rutin bakım mı?
        /// </summary>
        public bool IsRoutine => Type == MaintenanceType.Routine;

        /// <summary>
        /// Yüksek maliyetli bakım mı?
        /// </summary>
        public bool IsHighCost => EstimatedCost > 5000;

        /// <summary>
        /// Sonraki bakım bilgisi var mı?
        /// </summary>
        public bool HasNextMaintenanceInfo => NextMaintenanceDate.HasValue || NextMaintenanceKm.HasValue;

        /// <summary>
        /// Kilometre bazlı sonraki bakım mı?
        /// </summary>
        public bool IsKmBasedNextMaintenance => NextMaintenanceKm.HasValue;

        /// <summary>
        /// Tarih bazlı sonraki bakım mı?
        /// </summary>
        public bool IsDateBasedNextMaintenance => NextMaintenanceDate.HasValue;

        /// <summary>
        /// Validation uyarıları
        /// </summary>
        public List<string> GetValidationWarnings()
        {
            var warnings = new List<string>();

            if (IsBackdated)
                warnings.Add("Geçmiş tarihli bakım oluşturuyorsunuz");

            if (IsFarFuture)
                warnings.Add("Bakım 6 aydan uzak bir tarihte planlanıyor");

            if (IsEmergency && !IsNearFuture)
                warnings.Add("Acil bakım yakın tarihe planlanmalıdır");

            if (IsHighCost && !EstimatedCost.HasValue)
                warnings.Add("Yüksek maliyetli bakım için tahmini maliyet girilmelidir");

            if (IsRoutine && !HasNextMaintenanceInfo)
                warnings.Add("Rutin bakım için sonraki bakım bilgisi önerilir");

            if (NextMaintenanceKm.HasValue && NextMaintenanceKm <= VehicleKm)
                warnings.Add("Sonraki bakım kilometresi mevcut kilometreden büyük olmalıdır");

            return warnings;
        }

        /// <summary>
        /// Öncelik puanı hesaplama
        /// </summary>
        public int CalculatePriorityScore()
        {
            var score = 0;

            // Bakım türüne göre puan
            score += Type switch
            {
                MaintenanceType.Emergency => 100,
                MaintenanceType.Corrective => 80,
                MaintenanceType.Preventive => 60,
                MaintenanceType.Routine => 40,
                MaintenanceType.Inspection => 30,
                _ => 20
            };

            // Yakınlığa göre puan
            var daysUntil = (ScheduledDate.Date - DateTime.Today).Days;
            if (daysUntil <= 1) score += 50;
            else if (daysUntil <= 3) score += 30;
            else if (daysUntil <= 7) score += 20;

            // Öncelik flagine göre puan
            if (IsPriority) score += 25;

            return Math.Min(200, score); // Maksimum 200 puan
        }
    }
}