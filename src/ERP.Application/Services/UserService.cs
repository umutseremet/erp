using AutoMapper;
using ERP.Application.DTOs.User;
using ERP.Application.Common.Models;
using ERP.Application.Interfaces.Services;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using ERP.Core.Exceptions;

namespace ERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<UserDto>.Failure("Kullanıcı bulunamadı");

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(email);
                if (user == null)
                    return Result<UserDto>.Failure("Kullanıcı bulunamadı");

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> GetUserByRedmineIdAsync(int redmineUserId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByRedmineUserIdAsync(redmineUserId);
                if (user == null)
                    return Result<UserDto>.Failure("Kullanıcı bulunamadı");

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> GetUserByEmployeeNumberAsync(string employeeNumber)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByEmployeeNumberAsync(employeeNumber);
                if (user == null)
                    return Result<UserDto>.Failure("Kullanıcı bulunamadı");

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<UserDto>> GetUsersPagedAsync(UserFilterDto filter, int pageNumber, int pageSize)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();

                // Filtreleme
                 

                if (filter.Status.HasValue)
                    users = users.Where(u => u.Status == filter.Status.Value);

                if (filter.DepartmentId.HasValue)
                    users = users.Where(u => u.UserDepartments.Any(ud => ud.DepartmentId == filter.DepartmentId.Value && ud.IsActive));

                if (filter.CreatedAfter.HasValue)
                    users = users.Where(u => u.CreatedAt >= filter.CreatedAfter.Value);

                if (filter.CreatedBefore.HasValue)
                    users = users.Where(u => u.CreatedAt <= filter.CreatedBefore.Value);

                if (filter.LastLoginAfter.HasValue)
                    users = users.Where(u => u.LastLoginDate >= filter.LastLoginAfter.Value);

                 
                 
                var totalCount = users.Count();
                var paginatedUsers = users
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var userDtos = _mapper.Map<IEnumerable<UserDto>>(paginatedUsers);

                return new PaginatedResult<UserDto>
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<UserDto>
                {
                    Items = new List<UserDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = 0, 
                };
            }
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserDto>>.Failure($"Kullanıcılar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<UserDto>>> GetActiveUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.Users.GetActiveUsersAsync();
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserDto>>.Failure($"Aktif kullanıcılar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<UserDto>>> GetUsersByDepartmentAsync(int departmentId)
        {
            try
            {
                var users = await _unitOfWork.Users.GetUsersByDepartmentAsync(departmentId);
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserDto>>.Failure($"Departman kullanıcıları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<UserDto>>> GetUsersByRoleAsync(int roleId)
        {
            try
            {
                var roles = await _unitOfWork.Repository<Role>().FindAsync(r => r.Id == roleId);
                var role = roles.FirstOrDefault();
                if (role == null)
                    return Result<IEnumerable<UserDto>>.Failure("Rol bulunamadı");

                var users = role.UserRoles.Where(ur => ur.IsActive && !ur.IsExpired).Select(ur => ur.User);
                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return Result<IEnumerable<UserDto>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<UserDto>>.Failure($"Rol kullanıcıları getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto)
        {
            try
            {
                // Validation
                if (await _unitOfWork.Users.IsEmailExistsAsync(dto.Email))
                    return Result<UserDto>.Failure("Bu email adresi zaten kullanımda");

                if (!string.IsNullOrEmpty(dto.EmployeeNumber) &&
                    await _unitOfWork.Users.IsEmployeeNumberExistsAsync(dto.EmployeeNumber))
                    return Result<UserDto>.Failure("Bu personel numarası zaten kullanımda");

                var user = new User(
                    dto.RedmineUserId,
                    dto.FirstName,
                    dto.LastName,
                    dto.Email,
                    dto.PhoneNumber,
                    dto.EmployeeNumber);

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<UserDto>.Failure("Kullanıcı bulunamadı");

                // Email kontrolü (kendisi hariç)
                if (await _unitOfWork.Users.IsEmailExistsAsync(dto.Email, id))
                    return Result<UserDto>.Failure("Bu email adresi zaten kullanımda");

                user.SetName(dto.FirstName, dto.LastName);
                user.SetEmail(dto.Email);
                user.SetPhoneNumber(dto.PhoneNumber);

                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var userDto = _mapper.Map<UserDto>(user);
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"Kullanıcı güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                // Aktif araç ataması kontrolü
                if (user.AssignedVehicles.Any(v => v.Status == VehicleStatus.Assigned))
                    return Result<bool>.Failure("Kullanıcının aktif araç ataması bulunduğu için silinemez");

                user.MarkAsDeleted();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı silinirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ActivateUserAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                user.Activate();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı aktive edilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeactivateUserAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                user.Deactivate();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı deaktive edilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SuspendUserAsync(int id, string reason)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                user.Suspend();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı askıya alınırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UnsuspendUserAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(id);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                user.Activate();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı askıdan çıkarılırken hata oluştu: {ex.Message}");
            }
        }

        // Diğer metodlar için placeholder implementation
        public async Task<Result<bool>> RemoveUserFromDepartmentAsync(int userId, int departmentId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                var userDepartment = user.UserDepartments.FirstOrDefault(ud => ud.DepartmentId == departmentId && ud.IsActive);
                if (userDepartment == null)
                    return Result<bool>.Failure("Kullanıcının bu departmanı bulunmuyor");

                userDepartment.End(DateTime.UtcNow);
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı departmandan çıkarılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(userId);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                user.UpdateLastLogin();
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Son giriş zamanı güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsEmailUniqueAsync(string email, int? excludeUserId = null)
        {
            try
            {
                var exists = await _unitOfWork.Users.IsEmailExistsAsync(email, excludeUserId);
                return Result<bool>.Success(!exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Email kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsEmployeeNumberUniqueAsync(string employeeNumber, int? excludeUserId = null)
        {
            try
            {
                var exists = await _unitOfWork.Users.IsEmployeeNumberExistsAsync(employeeNumber, excludeUserId);
                return Result<bool>.Success(!exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Personel numarası kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> IsRedmineUserIdUniqueAsync(int redmineUserId, int? excludeUserId = null)
        {
            try
            {
                var existingUser = await _unitOfWork.Users.GetByRedmineUserIdAsync(redmineUserId);
                var isUnique = existingUser == null || (excludeUserId.HasValue && existingUser.Id == excludeUserId.Value);
                return Result<bool>.Success(isUnique);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Redmine kullanıcı ID kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        // Diğer metodların placeholder implementasyonları
        public Task<Result<IEnumerable<UserDto>>> GetUserDepartmentsAsync(int userId) => throw new NotImplementedException();
        public Task<Result<bool>> RemoveRoleFromUserAsync(int userId, int roleId) => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetUserRolesAsync(int userId) => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetUserPermissionsAsync(int userId) => throw new NotImplementedException();
        public Task<Result<bool>> HasPermissionAsync(int userId, string permission) => throw new NotImplementedException();
        public Task<Result<bool>> AssignVehicleToUserAsync(int userId, int vehicleId) => throw new NotImplementedException();
        public Task<Result<bool>> UnassignVehicleFromUserAsync(int userId, int vehicleId) => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetUserVehiclesAsync(int userId) => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetUserSessionsAsync(int userId) => throw new NotImplementedException();
        public Task<Result<bool>> TerminateAllSessionsAsync(int userId) => throw new NotImplementedException();
        public Task<Result<UserStatisticsDto>> GetUserStatisticsAsync() => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetOverdueUsersAsync() => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetUserDepartmentDistributionAsync() => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> GetUserRoleDistributionAsync() => throw new NotImplementedException();
        public Task<Result<IEnumerable<UserDto>>> ExportUsersAsync(UserExportFilterDto filter) => throw new NotImplementedException();
        public Task<Result<bool>> ImportUsersAsync(byte[] data, UserImportFormat format) => throw new NotImplementedException();
        public Task<Result<string>> GetImportTemplateAsync(UserImportFormat format) => throw new NotImplementedException();
        public Task<Result<IEnumerable<CreateUserDto>>> CreateMultipleUsersAsync(IEnumerable<CreateUserDto> users) => throw new NotImplementedException();
        public Task<Result<IEnumerable<UpdateUserBulkDto>>> UpdateMultipleUsersAsync(IEnumerable<UpdateUserBulkDto> users) => throw new NotImplementedException();
    }
}