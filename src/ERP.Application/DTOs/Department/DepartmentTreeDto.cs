using ERP.Application.DTOs.Common;

namespace ERP.Application.DTOs.Department
{
    public class DepartmentTreeDto : BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
        public List<DepartmentTreeDto> Children { get; set; } = new List<DepartmentTreeDto>();
        public bool HasChildren => Children.Any();
        public int Level { get; set; }
    }
}