using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Application.UseCases.User.Queries;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class GetUsersByDepartmentHandler : IRequestHandler<GetUsersByDepartmentQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersByDepartmentHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersByDepartmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersByDepartmentAsync(request.DepartmentId);

                if (!request.IncludeInactive)
                    users = users.Where(u => u.Status == UserStatus.Active);

                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserDto>>.Failure($"Departman kullanıcıları getirilirken hata: {ex.Message}");
            }
        }
    }
}