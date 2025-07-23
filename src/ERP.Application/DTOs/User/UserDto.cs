// DTOs/User/UserDto.cs
using ERP.Application.DTOs.Common;
using ERP.Application.DTOs.Department;
using ERP.Application.DTOs.Vehicle;

namespace ERP.Application.DTOs.User
{
    public class UserDto : BaseDto
    {
        public int RedmineUserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public List<DepartmentDto>? Departments { get; set; }
        public List<VehicleDto>? AssignedVehicles { get; set; }
    }
}