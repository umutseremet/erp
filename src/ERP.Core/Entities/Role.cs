using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; private set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; private set; }
        
        public bool IsSystemRole { get; private set; } = false;
        
        public bool IsActive { get; private set; } = true;

        // Navigation Properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        protected Role() { }

        public Role(string name, string? description = null)
        {
            SetName(name);
            Description = description?.Trim();
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Rol adı boş olamaz");

            Name = name.Trim();
            UpdateTimestamp();
        }

        public void SetDescription(string? description)
        {
            Description = description?.Trim();
            UpdateTimestamp();
        }

        public void MarkAsSystem()
        {
            IsSystemRole = true;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();
        }

        public void Deactivate()
        {
            if (IsSystemRole)
                throw new InvalidOperationException("Sistem rolleri deaktive edilemez");

            IsActive = false;
            UpdateTimestamp();
        }

        public void AddPermission(int permissionId)
        {
            if (RolePermissions.Any(rp => rp.PermissionId == permissionId))
                return;

            var rolePermission = new RolePermission(Id, permissionId);
            RolePermissions.Add(rolePermission);
            UpdateTimestamp();
        }

        public void RemovePermission(int permissionId)
        {
            var rolePermission = RolePermissions.FirstOrDefault(rp => rp.PermissionId == permissionId);
            if (rolePermission != null)
            {
                RolePermissions.Remove(rolePermission);
                UpdateTimestamp();
            }
        }

        public bool HasPermission(int permissionId)
        {
            return RolePermissions.Any(rp => rp.PermissionId == permissionId);
        }
    }
}