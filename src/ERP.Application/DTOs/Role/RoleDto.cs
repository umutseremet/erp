using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.User;

public class RoleDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
    public bool IsActive { get; set; }
    public int UserCount { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}