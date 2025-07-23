using ERP.Core.Enums;

namespace ERP.Application.DTOs.Permission
{
    public class CreatePermissionDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public PermissionType Type { get; set; }
    }
}
