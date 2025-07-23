using AutoMapper;
using ERP.Application.Common.Constants;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Application.Interfaces.Infrastructure;
using ERP.Application.UseCases.User.Commands;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
                if (user == null)
                    return Result<UserDto>.Failure("Kullanıcı bulunamadı");

                // Check email uniqueness
                var emailExists = await _unitOfWork.Users.IsEmailExistsAsync(request.Email, request.Id);
                if (emailExists)
                    return Result<UserDto>.Failure("Email adresi zaten kullanımda");

                // Check employee number uniqueness
                if (!string.IsNullOrEmpty(request.EmployeeNumber))
                {
                    var employeeNumberExists = await _unitOfWork.Users.IsEmployeeNumberExistsAsync(request.EmployeeNumber, request.Id);
                    if (employeeNumberExists)
                        return Result<UserDto>.Failure("Çalışan numarası zaten kullanımda");
                }

                user.SetName(request.FirstName, request.LastName);
                user.SetEmail(request.Email);
                user.SetPhoneNumber(request.PhoneNumber);
                user.SetStatus(request.Status);

                await _unitOfWork.SaveChangesAsync();

                // Clear cache
                await _cacheService.RemoveAsync(CacheKeys.UserById(request.Id));

                var dto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı güncellenirken hata: {ex.Message}");
            }
        }
    }
}