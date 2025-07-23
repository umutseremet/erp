using ERP.Application.DTOs.Department;
using ERP.Application.Common.Models;
using ERP.Application.Interfaces.Services;
using ERP.Application.Interfaces.Infrastructure;
using ERP.Core.Interfaces;
using ERP.Core.Entities;
using AutoMapper;

namespace ERP.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Result<DepartmentDto>> GetDepartmentByIdAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return Result<DepartmentDto>.Failure("Departman bulunamadı.");
                }

                var departmentDto = _mapper.Map<DepartmentDto>(department);
                return Result<DepartmentDto>.Success(departmentDto);
            }
            catch (Exception ex)
            {
                return Result<DepartmentDto>.Failure($"Departman getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<DepartmentDto>> GetDepartmentByCodeAsync(string code)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByCodeAsync(code);
                if (department == null)
                {
                    return Result<DepartmentDto>.Failure("Departman bulunamadı.");
                }

                var departmentDto = _mapper.Map<DepartmentDto>(department);
                return Result<DepartmentDto>.Success(departmentDto);
            }
            catch (Exception ex)
            {
                return Result<DepartmentDto>.Failure($"Departman getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<PaginatedResult<DepartmentDto>> GetDepartmentsPagedAsync(DepartmentFilterDto filter, int pageNumber, int pageSize)
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();

                // Apply filters
                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    departments = departments.Where(d =>
                        d.Name.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        (d.Code != null && d.Code.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase)));
                }

                if (filter.IsActive.HasValue)
                {
                    departments = departments.Where(d => d.IsActive == filter.IsActive.Value);
                }

                if (filter.ParentDepartmentId.HasValue)
                {
                    departments = departments.Where(d => d.ParentDepartmentId == filter.ParentDepartmentId.Value);
                }

                var totalCount = departments.Count();
                var pagedDepartments = departments
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(pagedDepartments);

                return new PaginatedResult<DepartmentDto>
                {
                    Items = departmentDtos,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResult<DepartmentDto>
                {
                    Items = new List<DepartmentDto>(),
                    TotalCount = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAllAsync();
                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                return Result<IEnumerable<DepartmentDto>>.Success(departmentDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DepartmentDto>>.Failure($"Departmanlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetActiveDepartmentsAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetActiveAsync();
                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                return Result<IEnumerable<DepartmentDto>>.Success(departmentDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DepartmentDto>>.Failure($"Aktif departmanlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetRootDepartmentsAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetRootDepartmentsAsync();
                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                return Result<IEnumerable<DepartmentDto>>.Success(departmentDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DepartmentDto>>.Failure($"Ana departmanlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetSubDepartmentsAsync(int parentId)
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetSubDepartmentsAsync(parentId);
                var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
                return Result<IEnumerable<DepartmentDto>>.Success(departmentDtos);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DepartmentDto>>.Failure($"Alt departmanlar getirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<DepartmentTreeDto>>> GetDepartmentTreeAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetActiveAsync();
                var rootDepartments = departments.Where(d => d.ParentDepartmentId == null);

                var tree = new List<DepartmentTreeDto>();
                foreach (var root in rootDepartments)
                {
                    var treeNode = BuildDepartmentTree(root, departments.ToList());
                    tree.Add(treeNode);
                }

                return Result<IEnumerable<DepartmentTreeDto>>.Success(tree);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<DepartmentTreeDto>>.Failure($"Departman ağacı oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        private DepartmentTreeDto BuildDepartmentTree(Department department, List<Department> allDepartments)
        {
            var treeDto = _mapper.Map<DepartmentTreeDto>(department);
            var children = allDepartments.Where(d => d.ParentDepartmentId == department.Id);

            treeDto.Children = children.Select(child => BuildDepartmentTree(child, allDepartments)).ToList();
            return treeDto;
        }

        public async Task<Result<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            try
            {
                // Validation
                if (!string.IsNullOrEmpty(dto.Code))
                {
                    var codeExists = await _unitOfWork.Departments.IsCodeExistsAsync(dto.Code);
                    if (codeExists)
                    {
                        return Result<DepartmentDto>.Failure("Bu departman kodu zaten kullanılıyor.");
                    }
                }

                var department = new Department(dto.Name, dto.Description, dto.Code, dto.ParentDepartmentId);

                await _unitOfWork.Departments.AddAsync(department);
                await _unitOfWork.SaveChangesAsync();

                var departmentDto = _mapper.Map<DepartmentDto>(department);
                return Result<DepartmentDto>.Success(departmentDto);
            }
            catch (Exception ex)
            {
                return Result<DepartmentDto>.Failure($"Departman oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<DepartmentDto>> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return Result<DepartmentDto>.Failure("Departman bulunamadı.");
                }

                // Code uniqueness check
                if (!string.IsNullOrEmpty(dto.Code) && department.Code != dto.Code)
                {
                    var codeExists = await _unitOfWork.Departments.IsCodeExistsAsync(dto.Code, id);
                    if (codeExists)
                    {
                        return Result<DepartmentDto>.Failure("Bu departman kodu zaten kullanılıyor.");
                    }
                }

                department.SetName(dto.Name);
                department.SetDescription(dto.Description);
                department.SetCode(dto.Code);

                if (dto.IsActive)
                    department.Activate();
                else
                    department.Deactivate();

                await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.SaveChangesAsync();

                var departmentDto = _mapper.Map<DepartmentDto>(department);
                return Result<DepartmentDto>.Success(departmentDto);
            }
            catch (Exception ex)
            {
                return Result<DepartmentDto>.Failure($"Departman güncellenirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteDepartmentAsync(int id)
        {
            try
            {
                var canDelete = await CanDeleteDepartmentAsync(id);
                if (!canDelete.IsSuccess || !canDelete.Data)
                {
                    return Result<bool>.Failure("Departman silinemez. Alt departmanları veya kullanıcıları bulunuyor.");
                }

                await _unitOfWork.Departments.DeleteByIdAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Departman silinirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ActivateDepartmentAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return Result<bool>.Failure("Departman bulunamadı.");
                }

                department.Activate();
                await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Departman aktifleştirilirken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeactivateDepartmentAsync(int id)
        {
            try
            {
                var department = await _unitOfWork.Departments.GetByIdAsync(id);
                if (department == null)
                {
                    return Result<bool>.Failure("Departman bulunamadı.");
                }

                department.Deactivate();
                await _unitOfWork.Departments.UpdateAsync(department);
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Departman deaktifleştirilirken hata oluştu: {ex.Message}");
            }
        }

        // Placeholder implementations for remaining methods
        public async Task<Result<IEnumerable<DepartmentDto>>> GetDepartmentHierarchyAsync(int departmentId)
        {
            // Implementation needed
            return Result<IEnumerable<DepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ReorganizeDepartmentAsync(int id, int? newParentId)
        {
            // Implementation needed
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetDepartmentUsersAsync(int id)
        {
            // Implementation needed
            return Result<IEnumerable<DepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> AssignUserToDepartmentAsync(int userId, int departmentId, bool isPrimary = false)
        {
            // Implementation needed
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> RemoveUserFromDepartmentAsync(int userId, int departmentId)
        {
            // Implementation needed
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ChangePrimaryDepartmentAsync(int userId, int departmentId)
        {
            // Implementation needed
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> TransferUserToDepartmentAsync(int userId, int fromDepartmentId, int toDepartmentId)
        {
            // Implementation needed
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> CanDeleteDepartmentAsync(int id)
        {
            try
            {
                var hasSubDepartments = await _unitOfWork.Departments.HasSubDepartmentsAsync(id);
                var hasUsers = await _unitOfWork.Departments.HasUsersAsync(id);

                return Result<bool>.Success(!hasSubDepartments && !hasUsers);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Silinebilirlik kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> HasSubDepartmentsAsync(int id)
        {
            try
            {
                var hasSubDepartments = await _unitOfWork.Departments.HasSubDepartmentsAsync(id);
                return Result<bool>.Success(hasSubDepartments);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Alt departman kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<bool>> HasUsersAsync(int id)
        {
            try
            {
                var hasUsers = await _unitOfWork.Departments.HasUsersAsync(id);
                return Result<bool>.Success(hasUsers);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı kontrolü yapılırken hata oluştu: {ex.Message}");
            }
        }

        public async Task<Result<DepartmentStatsDto>> GetDepartmentStatisticsAsync(int id)
        {
            // Implementation needed
            return Result<DepartmentStatsDto>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetOverallDepartmentStatisticsAsync()
        {
            // Implementation needed
            return Result<IEnumerable<DepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetUserDistributionAsync()
        {
            // Implementation needed
            return Result<IEnumerable<DepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> GetDepartmentHierarchyAsync()
        {
            // Implementation needed
            return Result<IEnumerable<DepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<DepartmentDto>>> ExportDepartmentsAsync(DepartmentExportFormat format)
        {
            // Implementation needed
            return Result<IEnumerable<DepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<bool>> ImportDepartmentsAsync(byte[] data, DepartmentImportFormat format)
        {
            // Implementation needed
            return Result<bool>.Failure("Method not implemented yet");
        }

        public async Task<Result<string>> GetImportTemplateAsync(DepartmentImportFormat format)
        {
            // Implementation needed
            return Result<string>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<CreateDepartmentDto>>> CreateMultipleDepartmentsAsync(IEnumerable<CreateDepartmentDto> departments)
        {
            // Implementation needed
            return Result<IEnumerable<CreateDepartmentDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<UpdateDepartmentBulkDto>>> UpdateMultipleDepartmentsAsync(IEnumerable<UpdateDepartmentBulkDto> departments)
        {
            // Implementation needed
            return Result<IEnumerable<UpdateDepartmentBulkDto>>.Failure("Method not implemented yet");
        }

        public async Task<Result<IEnumerable<int>>> DeleteMultipleDepartmentsAsync(IEnumerable<int> departmentIds)
        {
            // Implementation needed
            return Result<IEnumerable<int>>.Failure("Method not implemented yet");
        }
    }
}