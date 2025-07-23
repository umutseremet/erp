using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Core.Enums;
using MediatR;

namespace ERP.Application.UseCases.User.Commands
{
    public class UpdateUserCommand : IRequest<Result<UserDto>>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? EmployeeNumber { get; set; }
        public UserStatus Status { get; set; }
    }
}