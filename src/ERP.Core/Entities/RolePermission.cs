namespace ERP.Core.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }

        // Navigation Properties
        public virtual Role Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;

        protected RolePermission() { }

        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}