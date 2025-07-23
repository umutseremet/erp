using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.User.Commands
{
    public class DeleteUserCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
}
