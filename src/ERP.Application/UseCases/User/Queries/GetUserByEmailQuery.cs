using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using MediatR;

namespace ERP.Application.UseCases.User.Queries
{
    public class GetUserByEmailQuery : IRequest<Result<UserDto?>>
    {
        public string Email { get; set; } = string.Empty;
    }
}