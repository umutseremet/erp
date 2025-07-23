namespace ERP.Application.DTOs.Department
{
    public class UpdateDepartmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}