using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using MediatR;

namespace ERP.Application.UseCases.User.Commands
{
    public class CreateUserCommand : IRequest<Result<UserDto>>
    {
        public int RedmineUserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public int? PrimaryDepartmentId { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}