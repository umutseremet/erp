using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using MediatR;

namespace ERP.Application.UseCases.User.Queries
{
    public class GetUserByIdQuery : IRequest<Result<UserProfileDto?>>
    {
        public int Id { get; set; }
    }
}