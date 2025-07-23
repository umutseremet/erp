using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;
using ERP.Core.ValueObjects;

namespace ERP.Core.Entities
{
    public class VehicleMaintenance : BaseEntity
    {
        public int VehicleId { get; private set; }
        
        public MaintenanceType Type { get; private set; }
        
        public DateTime ScheduledDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        
        public decimal VehicleKm { get; private set; }
        
        [Required]
        [StringLength(200)]
        public string Description { get; private set; } = string.Empty;
        
        [StringLength(200)]
        public string? ServiceProvider { get; private set; }
        
        public decimal? Cost { get; private set; }
        
        public DateTime? NextMaintenanceDate { get; private set; }
        public decimal? NextMaintenanceKm { get; private set; }
        
        [StringLength(1000)]
        public string? Notes { get; private set; }
        
        public bool IsCompleted { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;

        protected VehicleMaintenance() { }

        public VehicleMaintenance(int vehicleId, MaintenanceType type, DateTime scheduledDate, 
                                decimal vehicleKm, string description, string? serviceProvider = null)
        {
            VehicleId = vehicleId;
            Type = type;
            ScheduledDate = scheduledDate;
            VehicleKm = vehicleKm;
            SetDescription(description);
            ServiceProvider = serviceProvider?.Trim();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Bakım açıklaması boş olamaz");

            Description = description.Trim();
            UpdateTimestamp();
        }

        public void SetCost(decimal cost)
        {
            if (cost < 0)
                throw new ArgumentException("Maliyet negatif olamaz");

            Cost = cost;
            UpdateTimestamp();
        }

        public void Complete(DateTime completedDate, decimal? cost = null, string? notes = null)
        {
            if (IsCompleted)
                throw new InvalidOperationException("Bakım zaten tamamlanmış");

            CompletedDate = completedDate;
            IsCompleted = true;
            
            if (cost.HasValue)
                SetCost(cost.Value);
            
            if (!string.IsNullOrWhiteSpace(notes))
                Notes = notes.Trim();
                
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public void ScheduleNextMaintenance(DateTime? nextDate, decimal? nextKm)
        {
            NextMaintenanceDate = nextDate;
            NextMaintenanceKm = nextKm;
            UpdateTimestamp();
        }

        public void Reschedule(DateTime newScheduledDate)
        {
            if (IsCompleted)
                throw new InvalidOperationException("Tamamlanmış bakım yeniden planlanamaz");

            ScheduledDate = newScheduledDate;
            UpdateTimestamp();
        }

        public bool IsOverdue => !IsCompleted && DateTime.UtcNow > ScheduledDate;

        public Money GetCostAsMoney(string currency = "TRY")
        {
            return Cost.HasValue ? new Money(Cost.Value, currency) : new Money(0, currency);
        }
    }
}