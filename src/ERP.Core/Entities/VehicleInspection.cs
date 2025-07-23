using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;

namespace ERP.Core.Entities
{
    public class VehicleInspection : BaseEntity
    {
        public int VehicleId { get; private set; }
        
        public DateTime InspectionDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        
        public InspectionStatus Status { get; private set; }
        
        [StringLength(200)]
        public string? InspectionCenter { get; private set; }
        
        [StringLength(100)]
        public string? CertificateNumber { get; private set; }
        
        public decimal VehicleKm { get; private set; }
        
        public decimal? Cost { get; private set; }
        
        [StringLength(1000)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;

        protected VehicleInspection() { }

        public VehicleInspection(int vehicleId, DateTime inspectionDate, DateTime expiryDate, 
                               InspectionStatus status, decimal vehicleKm)
        {
            VehicleId = vehicleId;
            InspectionDate = inspectionDate;
            ExpiryDate = expiryDate;
            Status = status;
            VehicleKm = vehicleKm;
        }

        public void SetStatus(InspectionStatus status)
        {
            Status = status;
            UpdateTimestamp();
        }

        public void SetInspectionCenter(string? inspectionCenter)
        {
            InspectionCenter = inspectionCenter?.Trim();
            UpdateTimestamp();
        }

        public void SetCertificateNumber(string? certificateNumber)
        {
            CertificateNumber = certificateNumber?.Trim();
            UpdateTimestamp();
        }

        public void SetCost(decimal? cost)
        {
            if (cost.HasValue && cost < 0)
                throw new ArgumentException("Maliyet negatif olamaz");

            Cost = cost;
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > ExpiryDate;
    }
}