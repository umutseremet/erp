using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using MediatR;

namespace ERP.Application.UseCases.User.Queries
{
    public class GetUsersByDepartmentQuery : IRequest<Result<IEnumerable<UserDto>>>
    {
        public int DepartmentId { get; set; }
        public bool IncludeInactive { get; set; } = false;
    }
}