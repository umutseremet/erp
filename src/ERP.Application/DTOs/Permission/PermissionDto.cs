using ERP.Application.DTOs.Common;

namespace ERP.Application.DTOs.Permission
{
    public class PermissionDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsSystemPermission { get; set; }
        public bool IsActive { get; set; }
        public string FullPermissionCode { get; set; } = string.Empty;
    }

    public enum PermissionType
    {
        Read = 1,
        Write = 2,
        Delete = 3,
        Approve = 4,
        Manage = 5,
        Admin = 6
    }
}