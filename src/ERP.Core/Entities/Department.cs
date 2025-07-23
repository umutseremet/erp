using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Entities
{
    public class Department : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; private set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; private set; }
        
        [StringLength(10)]
        public string? Code { get; private set; }
        
        public int? ParentDepartmentId { get; private set; }
        
        public bool IsActive { get; private set; } = true;

        // Navigation Properties
        public virtual Department? ParentDepartment { get; set; }
        public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
        public virtual ICollection<UserDepartment> UserDepartments { get; set; } = new List<UserDepartment>();

        protected Department() { }

        public Department(string name, string? description = null, string? code = null, int? parentDepartmentId = null)
        {
            SetName(name);
            Description = description;
            Code = code;
            ParentDepartmentId = parentDepartmentId;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Departman adı boş olamaz");
            
            Name = name.Trim();
            UpdateTimestamp();
        }

        public void SetDescription(string? description)
        {
            Description = description?.Trim();
            UpdateTimestamp();
        }

        public void SetCode(string? code)
        {
            Code = code?.Trim().ToUpperInvariant();
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
    }
}