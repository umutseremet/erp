using ERP.Core.Enums;

namespace ERP.Application.DTOs.User
{
    public class UserFilterDto
    {
        public string? SearchTerm { get; set; }
        public UserStatus? Status { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoleId { get; set; }
        public bool? HasAssignedVehicles { get; set; }
        public DateTime? LastLoginAfter { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }
}
