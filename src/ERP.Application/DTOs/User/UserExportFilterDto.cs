using ERP.Application.Interfaces.Services;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.User
{
    public class UserExportFilterDto
    {
        public UserStatus? Status { get; set; }
        public int? DepartmentId { get; set; }
        public bool IncludeVehicleAssignments { get; set; }
        public bool IncludeDepartmentInfo { get; set; }
        public UserExportFormat Format { get; set; } = UserExportFormat.Excel;
    }
}
