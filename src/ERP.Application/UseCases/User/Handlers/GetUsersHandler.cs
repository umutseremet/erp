using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.User;
using ERP.Application.UseCases.User.Queries;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, Result<PaginatedResult<UserListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedResult<UserListDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllAsync();

                // Apply filters
                //if (!string.IsNullOrEmpty(request.Filter.FirstName))
                //    users = users.Where(u => u.FirstName.Contains(request.Filter.FirstName, StringComparison.OrdinalIgnoreCase));

                //if (!string.IsNullOrEmpty(request.Filter.LastName))
                //    users = users.Where(u => u.LastName.Contains(request.Filter.LastName, StringComparison.OrdinalIgnoreCase));

                //if (!string.IsNullOrEmpty(request.Filter.Email))
                //    users = users.Where(u => u.Email.Contains(request.Filter.Email, StringComparison.OrdinalIgnoreCase));

                if (request.Filter.Status.HasValue)
                    users = users.Where(u => u.Status == request.Filter.Status.Value);

                if (request.Filter.DepartmentId.HasValue)
                    users = users.Where(u => u.UserDepartments.Any(ud => ud.DepartmentId == request.Filter.DepartmentId.Value && ud.IsActive));

                //// Apply sorting
                //users = request.Filter.SortBy?.ToLower() switch
                //{
                //    "firstname" => request.Filter.SortDescending ? users.OrderByDescending(u => u.FirstName) : users.OrderBy(u => u.FirstName),
                //    "lastname" => request.Filter.SortDescending ? users.OrderByDescending(u => u.LastName) : users.OrderBy(u => u.LastName),
                //    "email" => request.Filter.SortDescending ? users.OrderByDescending(u => u.Email) : users.OrderBy(u => u.Email),
                //    "createdat" => request.Filter.SortDescending ? users.OrderByDescending(u => u.CreatedAt) : users.OrderBy(u => u.CreatedAt),
                //    _ => users.OrderBy(u => u.FirstName)
                //};

                var totalCount = users.Count();
                var pagedUsers = users.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

                var userListDtos = _mapper.Map<IEnumerable<UserListDto>>(pagedUsers);

                var paginatedResult = new PaginatedResult<UserListDto>(
                    userListDtos,
                    totalCount,
                    request.Page,
                    request.PageSize
                );

                return Result<PaginatedResult<UserListDto>>.Success(paginatedResult);
            }
            catch (Exception ex)
            {
                return Result<PaginatedResult<UserListDto>>.Failure($"Kullanıcılar getirilirken hata: {ex.Message}");
            }
        }
    }
}