namespace ERP.Core.Entities
{
    public class UserDepartment : BaseEntity
    {
        public int UserId { get; private set; }
        public int DepartmentId { get; private set; }
        
        public DateTime AssignedDate { get; private set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; private set; }
        
        public bool IsPrimary { get; private set; } = false;
        public bool IsActive { get; private set; } = true;

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual Department Department { get; set; } = null!;

        protected UserDepartment() { }

        public UserDepartment(int userId, int departmentId, bool isPrimary = false)
        {
            UserId = userId;
            DepartmentId = departmentId;
            IsPrimary = isPrimary;
        }

        public void SetAsPrimary()
        {
            IsPrimary = true;
            UpdateTimestamp();
        }

        public void SetAsSecondary()
        {
            IsPrimary = false;
            UpdateTimestamp();
        }

        public void End(DateTime endDate)
        {
            EndDate = endDate;
            IsActive = false;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            EndDate = null;
            UpdateTimestamp();
        }

        public bool IsCurrentAssignment => IsActive && (!EndDate.HasValue || EndDate.Value > DateTime.UtcNow);
    }
}