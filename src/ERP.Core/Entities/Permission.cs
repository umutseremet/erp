using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;

namespace ERP.Core.Entities
{
    public class Permission : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Description { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Module { get; private set; } = string.Empty; // Vehicle, User, Fuel, etc.
        
        [Required]
        [StringLength(50)]
        public string Action { get; private set; } = string.Empty; // Create, Read, Update, Delete, etc.
        
        public PermissionType Type { get; private set; }
        
        public bool IsSystemPermission { get; private set; } = false;
        
        public bool IsActive { get; private set; } = true;

        // Navigation Properties
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        protected Permission() { }

        public Permission(string name, string description, string module, string action, PermissionType type)
        {
            SetName(name);
            SetDescription(description);
            SetModule(module);
            SetAction(action);
            Type = type;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Yetki adı boş olamaz");

            Name = name.Trim();
            UpdateTimestamp();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Yetki açıklaması boş olamaz");

            Description = description.Trim();
            UpdateTimestamp();
        }

        public void SetModule(string module)
        {
            if (string.IsNullOrWhiteSpace(module))
                throw new ArgumentException("Modül adı boş olamaz");

            Module = module.Trim();
            UpdateTimestamp();
        }

        public void SetAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("İşlem adı boş olamaz");

            Action = action.Trim();
            UpdateTimestamp();
        }

        public void MarkAsSystem()
        {
            IsSystemPermission = true;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();
        }

        public void Deactivate()
        {
            if (IsSystemPermission)
                throw new InvalidOperationException("Sistem yetkileri deaktive edilemez");

            IsActive = false;
            UpdateTimestamp();
        }

        public string GetFullPermissionCode()
        {
            return $"{Module}.{Action}";
        }
    }
}