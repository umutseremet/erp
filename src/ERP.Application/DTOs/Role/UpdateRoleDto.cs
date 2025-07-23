namespace ERP.Application.DTOs.Role
{
    public class UpdateRoleDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
}
