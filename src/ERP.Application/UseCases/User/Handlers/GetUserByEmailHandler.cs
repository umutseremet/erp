using MediatR;
using ERP.Application.UseCases.User.Queries;
using ERP.Application.DTOs.User;
using ERP.Application.Common.Models;
using ERP.Core.Interfaces;
using AutoMapper;

namespace ERP.Application.UseCases.User.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, Result<UserDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByEmailHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserDto?>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
                if (user == null)
                    return Result<UserDto?>.Success(null);

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto?>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto?>.Failure($"Kullanıcı getirilirken hata: {ex.Message}");
            }
        }
    }
}