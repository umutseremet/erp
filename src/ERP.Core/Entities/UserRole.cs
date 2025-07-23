namespace ERP.Core.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; private set; }
        public int RoleId { get; private set; }
        
        public DateTime AssignedDate { get; private set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; private set; }
        
        public bool IsActive { get; private set; } = true;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;

        protected UserRole() { }

        public UserRole(int userId, int roleId, DateTime? expiryDate = null)
        {
            UserId = userId;
            RoleId = roleId;
            ExpiryDate = expiryDate;
        }

        public void SetExpiryDate(DateTime? expiryDate)
        {
            ExpiryDate = expiryDate;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdateTimestamp();
        }

        public bool IsExpired => ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate.Value;
        public bool IsValid => IsActive && !IsExpired;
    }
}