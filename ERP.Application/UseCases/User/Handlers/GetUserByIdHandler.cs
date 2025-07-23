using AutoMapper;
using ERP.Application.Common.Constants;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Application.Interfaces.Infrastructure;
using ERP.Application.UseCases.User.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserProfileDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetUserByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Result<UserProfileDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = CacheKeys.UserById(request.Id);
                var cachedUser = await _cacheService.GetAsync<UserProfileDto>(cacheKey);

                if (cachedUser != null)
                    return Result<UserProfileDto?>.Success(cachedUser);

                var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
                if (user == null)
                    return Result<UserProfileDto?>.Success(null);

                var userProfile = _mapper.Map<UserProfileDto>(user);

                // Load related data
                //userProfile.Departments = _mapper.Map<List<UserDepartmentDetailDto>>(user.UserDepartments.Where(ud => ud.IsActive));
                //userProfile.Roles = _mapper.Map<List<UserRoleDetailDto>>(user.UserRoles.Where(ur => ur.IsActive));
                //userProfile.AssignedVehicles = _mapper.Map<List<AssignedVehicleDetailDto>>(user.AssignedVehicles);

                await _cacheService.SetAsync(cacheKey, userProfile, TimeSpan.FromMinutes(30));

                return Result<UserProfileDto?>.Success(userProfile);
            }
            catch (Exception ex)
            {
                return Result<UserProfileDto?>.Failure($"Kullanıcı getirilirken hata: {ex.Message}");
            }
        }
    }
}