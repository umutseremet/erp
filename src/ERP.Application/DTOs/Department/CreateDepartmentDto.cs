using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Department
{
    public class CreateDepartmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
    }
}