// DTOs/User/CreateUserDto.cs
using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.User
{
    public class CreateUserDto
    {
        public int RedmineUserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public int? PrimaryDepartmentId { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }
}