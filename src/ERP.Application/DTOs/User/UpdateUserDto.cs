// DTOs/User/UpdateUserDto.cs
using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.User
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public UserStatus Status { get; set; }
        public int? PrimaryDepartmentId { get; set; }
    }
}