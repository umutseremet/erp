using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Department;
using ERP.Application.DTOs.Vehicle;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.User
{

    public class UserProfileDto : BaseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        public List<DepartmentDto> Departments { get; set; } = new();
        public List<RoleDto> Roles { get; set; } = new();
        public List<VehicleDto> AssignedVehicles { get; set; } = new();
        public List<PermissionDto> Permissions { get; set; } = new();
    }

    public class UserDepartmentDetailDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentCode { get; set; }
        public string? DepartmentDescription { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ParentDepartmentName { get; set; }
    }

    public class UserRoleDetailDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsExpired => ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate.Value;
        public bool IsValid => IsActive && !IsExpired;
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class PermissionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public PermissionType Type { get; set; }
    }

    public class AssignedVehicleDetailDto
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Color { get; set; }
        public VehicleType Type { get; set; }
        public VehicleStatus Status { get; set; }
        public decimal CurrentKm { get; set; }
        public DateTime AssignedDate { get; set; }
        public string VehicleInfo => $"{Year} {Brand} {Model}";
        public bool NeedsMaintenance { get; set; }
        public bool NeedsInspection { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime? NextInspectionDate { get; set; }
    }

    public class UserActivityDto
    {
        public int Id { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }
        public string? RelatedEntity { get; set; }
        public int? RelatedEntityId { get; set; }
        public string FormattedDate => ActivityDate.ToString("dd.MM.yyyy HH:mm");
    }
}