// 1. DTOs/Department/DepartmentDto.cs
using ERP.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.Department
{
    public class DepartmentDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
        public List<DepartmentDto>? SubDepartments { get; set; }
    }
}