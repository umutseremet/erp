using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Entities
{
    public class VehicleLicense : BaseEntity
    {
        public int VehicleId { get; private set; }
        
        [Required]
        [StringLength(100)]
        public string LicenseNumber { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string LicenseType { get; private set; } = string.Empty; // Ruhsat, İzin, vb.
        
        public DateTime IssueDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        
        [StringLength(200)]
        public string? IssuingAuthority { get; private set; }
        
        public bool IsActive { get; private set; } = true;
        
        [StringLength(1000)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;

        protected VehicleLicense() { }

        public VehicleLicense(int vehicleId, string licenseNumber, string licenseType, 
                            DateTime issueDate, DateTime expiryDate, string? issuingAuthority = null)
        {
            VehicleId = vehicleId;
            SetLicenseNumber(licenseNumber);
            SetLicenseType(licenseType);
            SetDates(issueDate, expiryDate);
            IssuingAuthority = issuingAuthority?.Trim();
        }

        public void SetLicenseNumber(string licenseNumber)
        {
            if (string.IsNullOrWhiteSpace(licenseNumber))
                throw new ArgumentException("Lisans numarası boş olamaz");

            LicenseNumber = licenseNumber.Trim();
            UpdateTimestamp();
        }

        public void SetLicenseType(string licenseType)
        {
            if (string.IsNullOrWhiteSpace(licenseType))
                throw new ArgumentException("Lisans türü boş olamaz");

            LicenseType = licenseType.Trim();
            UpdateTimestamp();
        }

        public void SetDates(DateTime issueDate, DateTime expiryDate)
        {
            if (expiryDate <= issueDate)
                throw new ArgumentException("Bitiş tarihi başlangıç tarihinden sonra olmalıdır");

            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            UpdateTimestamp();
        }

        public void Renew(DateTime newIssueDate, DateTime newExpiryDate, string? newLicenseNumber = null)
        {
            if (!string.IsNullOrWhiteSpace(newLicenseNumber))
                SetLicenseNumber(newLicenseNumber);
            
            SetDates(newIssueDate, newExpiryDate);
            IsActive = true;
            UpdateTimestamp();
        }

        public void Cancel()
        {
            IsActive = false;
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool IsExpiringSoon => DateTime.UtcNow.AddDays(30) > ExpiryDate;
        public int DaysUntilExpiry => (ExpiryDate - DateTime.UtcNow).Days;
    }
}