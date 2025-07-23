using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Application.Interfaces.Services;
using MediatR;

namespace ERP.Application.UseCases.User.Queries
{
    public class GetUsersQuery : IRequest<Result<PaginatedResult<UserListDto>>>
    {
        public UserFilterDto Filter { get; set; } = new UserFilterDto();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}