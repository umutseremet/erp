using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;
using ERP.Core.ValueObjects;

namespace ERP.Core.Entities
{
    public class User : BaseEntity
    {
        public int RedmineUserId { get; private set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; private set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; private set; } = string.Empty;
        
        public string? PhoneNumber { get; private set; }
        
        [StringLength(100)]
        public string? EmployeeNumber { get; private set; }
        
        public UserStatus Status { get; private set; } = UserStatus.Active;
        
        public DateTime? LastLoginDate { get; private set; }

        // Navigation Properties
        public virtual ICollection<UserDepartment> UserDepartments { get; set; } = new List<UserDepartment>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Vehicle> AssignedVehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<VehicleNotification> Notifications { get; set; } = new List<VehicleNotification>();

        protected User() { }

        public User(int redmineUserId, string firstName, string lastName, string email, string? phoneNumber = null, string? employeeNumber = null)
        {
            RedmineUserId = redmineUserId;
            SetName(firstName, lastName);
            SetEmail(email);
            SetPhoneNumber(phoneNumber);
            EmployeeNumber = employeeNumber;
        }

        public string FullName => $"{FirstName} {LastName}";

        public void SetName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ad boş olamaz");
            
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Soyad boş olamaz");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            UpdateTimestamp();
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email boş olamaz");

            Email = email.Trim().ToLowerInvariant();
            UpdateTimestamp();
        }

        public void SetPhoneNumber(string? phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                var phone = new PhoneNumber(phoneNumber);
                PhoneNumber = phone.Value;
            }
            else
            {
                PhoneNumber = null;
            }
            UpdateTimestamp();
        }

        public void SetStatus(UserStatus status)
        {
            Status = status;
            UpdateTimestamp();
        }

        public void UpdateLastLogin()
        {
            LastLoginDate = DateTime.UtcNow;
            UpdateTimestamp();
        }

        public void Activate()
        {
            Status = UserStatus.Active;
            UpdateTimestamp();
        }

        public void Deactivate()
        {
            Status = UserStatus.Inactive;
            UpdateTimestamp();
        }

        public void Suspend()
        {
            Status = UserStatus.Suspended;
            UpdateTimestamp();
        }
    }
}