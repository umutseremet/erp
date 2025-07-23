using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Application.Interfaces.Infrastructure;
using ERP.Application.UseCases.User.Commands;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public CreateUserHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validations
                var emailExists = await _unitOfWork.Users.IsEmailExistsAsync(request.Email);
                if (emailExists)
                    return Result<UserDto>.Failure("Email adresi zaten kullanımda");

                if (!string.IsNullOrEmpty(request.EmployeeNumber))
                {
                    var employeeNumberExists = await _unitOfWork.Users.IsEmployeeNumberExistsAsync(request.EmployeeNumber);
                    if (employeeNumberExists)
                        return Result<UserDto>.Failure("Çalışan numarası zaten kullanımda");
                }

                var redmineUserExists = await _unitOfWork.Users.GetByRedmineUserIdAsync(request.RedmineUserId);
                if (redmineUserExists != null)
                    return Result<UserDto>.Failure("Redmine kullanıcı ID zaten kullanımda");

                var user = new Core.Entities.User(
                    request.RedmineUserId,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber,
                    request.EmployeeNumber
                );

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Assign to primary department
                if (request.PrimaryDepartmentId.HasValue)
                {
                    var userDepartment = new UserDepartment(user.Id, request.PrimaryDepartmentId.Value, true);
                    await _unitOfWork.Repository<UserDepartment>().AddAsync(userDepartment);
                }

                // Assign roles
                foreach (var roleId in request.RoleIds)
                {
                    var userRole = new UserRole(user.Id, roleId);
                    await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
                }

                await _unitOfWork.SaveChangesAsync();

                var dto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı oluşturulurken hata: {ex.Message}");
            }
        }
    }
}
