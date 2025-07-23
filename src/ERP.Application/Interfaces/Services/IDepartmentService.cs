using ERP.Application.DTOs.Department;
using ERP.Application.Common.Models;

namespace ERP.Application.Interfaces.Services
{
    /// <summary>
    /// Departman yönetimi için servis interface'i
    /// </summary>
    /// 
    public interface IDepartmentService
    {
        Task<Result<DepartmentDto>> GetDepartmentByIdAsync(int id);
        Task<Result<DepartmentDto>> GetDepartmentByCodeAsync(string code);
        Task<PaginatedResult<DepartmentDto>> GetDepartmentsPagedAsync(DepartmentFilterDto filter, int pageNumber, int pageSize);
        Task<Result<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync();
        Task<Result<IEnumerable<DepartmentDto>>> GetActiveDepartmentsAsync();
        Task<Result<IEnumerable<DepartmentDto>>> GetRootDepartmentsAsync();
        Task<Result<IEnumerable<DepartmentDto>>> GetSubDepartmentsAsync(int parentId);
        Task<Result<IEnumerable<DepartmentTreeDto>>> GetDepartmentTreeAsync();
        Task<Result<IEnumerable<DepartmentDto>>> GetDepartmentHierarchyAsync(int departmentId);
        Task<Result<DepartmentDto>> CreateDepartmentAsync(CreateDepartmentDto dto);
        Task<Result<DepartmentDto>> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto);
        Task<Result<bool>> DeleteDepartmentAsync(int id);
        Task<Result<bool>> ActivateDepartmentAsync(int id);
        Task<Result<bool>> DeactivateDepartmentAsync(int id);
        Task<Result<bool>> ReorganizeDepartmentAsync(int id, int? newParentId);
        Task<Result<IEnumerable<DepartmentDto>>> GetDepartmentUsersAsync(int id);
        Task<Result<bool>> AssignUserToDepartmentAsync(int userId, int departmentId, bool isPrimary = false);
        Task<Result<bool>> RemoveUserFromDepartmentAsync(int userId, int departmentId);
        Task<Result<bool>> ChangePrimaryDepartmentAsync(int userId, int departmentId);
        Task<Result<bool>> TransferUserToDepartmentAsync(int userId, int fromDepartmentId, int toDepartmentId);
        Task<Result<bool>> CanDeleteDepartmentAsync(int id);
        Task<Result<bool>> HasSubDepartmentsAsync(int id);
        Task<Result<bool>> HasUsersAsync(int id);
        Task<Result<DepartmentStatsDto>> GetDepartmentStatisticsAsync(int id);
        Task<Result<IEnumerable<DepartmentDto>>> GetOverallDepartmentStatisticsAsync();
        Task<Result<IEnumerable<DepartmentDto>>> GetUserDistributionAsync();
        Task<Result<IEnumerable<DepartmentDto>>> GetDepartmentHierarchyAsync();
        Task<Result<IEnumerable<DepartmentDto>>> ExportDepartmentsAsync(DepartmentExportFormat format);
        Task<Result<bool>> ImportDepartmentsAsync(byte[] data, DepartmentImportFormat format);
        Task<Result<string>> GetImportTemplateAsync(DepartmentImportFormat format);
        Task<Result<IEnumerable<CreateDepartmentDto>>> CreateMultipleDepartmentsAsync(IEnumerable<CreateDepartmentDto> departments);
        Task<Result<IEnumerable<UpdateDepartmentBulkDto>>> UpdateMultipleDepartmentsAsync(IEnumerable<UpdateDepartmentBulkDto> departments);
        Task<Result<IEnumerable<int>>> DeleteMultipleDepartmentsAsync(IEnumerable<int> departmentIds);
    }
     
   

    /// <summary>
    /// Departman hiyerarşisi için DTO
    /// </summary>
    public class DepartmentHierarchyDto
    {
        public DepartmentDto Department { get; set; } = new DepartmentDto();
        public DepartmentDto? Parent { get; set; }
        public List<DepartmentDto> Ancestors { get; set; } = new List<DepartmentDto>();
        public List<DepartmentDto> Children { get; set; } = new List<DepartmentDto>();
        public int Level { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
    }

    public class DepartmentStatsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalSubDepartments { get; set; }
        public int ActiveSubDepartments { get; set; }
        public int TotalVehicles { get; set; }
        public int AssignedVehicles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    /// <summary>
    /// Departman kullanıcısı için DTO
    /// </summary>
    public class DepartmentUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string? EmployeeNumber { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Departman filtreleme için DTO
    /// </summary>
    public class DepartmentFilterDto
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IncludeSubDepartments { get; set; } = false;
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
    }

    /// <summary>
    /// Departman listesi için DTO
    /// </summary>
    public class DepartmentListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; }
        public int UserCount { get; set; }
        public int SubDepartmentCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormattedCreatedDate => CreatedAt.ToString("dd.MM.yyyy");
    }

    /// <summary>
    /// Departman detayı için DTO
    /// </summary>
    public class DepartmentDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? ParentDepartmentId { get; set; }
        public string? ParentDepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<DepartmentUserDto> Users { get; set; } = new List<DepartmentUserDto>();
        public List<DepartmentDto> SubDepartments { get; set; } = new List<DepartmentDto>();
        public DepartmentStatisticsDto Statistics { get; set; } = new DepartmentStatisticsDto();
    }

    /// <summary>
    /// Departman istatistikleri için DTO
    /// </summary>
    public class DepartmentStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int SubDepartmentCount { get; set; }
        public int TotalVehiclesAssigned { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public decimal TotalFuelCost { get; set; }
        public DateTime? LastUserAssignmentDate { get; set; }
    }

    /// <summary>
    /// Genel departman istatistikleri için DTO
    /// </summary>
    public class DepartmentOverallStatisticsDto
    {
        public int TotalDepartments { get; set; }
        public int ActiveDepartments { get; set; }
        public int InactiveDepartments { get; set; }
        public int RootDepartments { get; set; }
        public int MaxHierarchyLevel { get; set; }
        public decimal AverageUsersPerDepartment { get; set; }
        public DepartmentDto? LargestDepartment { get; set; }
        public DepartmentDto? SmallestDepartment { get; set; }
    }

    /// <summary>
    /// Departman kullanıcı dağılımı için DTO
    /// </summary>
    public class DepartmentUserDistributionDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Toplu işlem sonucu için DTO
    /// </summary>
    public class DepartmentBulkOperationResult
    {
        public int TotalCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<DepartmentBulkOperationError> Errors { get; set; } = new List<DepartmentBulkOperationError>();
        public bool IsSuccess => FailureCount == 0;
    }

    /// <summary>
    /// Toplu işlem hatası için DTO
    /// </summary>
    public class DepartmentBulkOperationError
    {
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
    }

    /// <summary>
    /// İçe aktarma sonucu için DTO
    /// </summary>
    public class DepartmentImportResult
    {
        public int TotalRecords { get; set; }
        public int SuccessfulImports { get; set; }
        public int FailedImports { get; set; }
        public List<DepartmentImportError> Errors { get; set; } = new List<DepartmentImportError>();
        public bool IsSuccess => FailedImports == 0;
    }

    /// <summary>
    /// İçe aktarma hatası için DTO
    /// </summary>
    public class DepartmentImportError
    {
        public int RowNumber { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Toplu güncelleme için DTO
    /// </summary>
    public class UpdateDepartmentBulkDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Code { get; set; }
        public int? ParentDepartmentId { get; set; }
        public bool IsActive { get; set; }
    }


    public enum DepartmentExportFormat
    {
        Excel,
        Csv,
        Json,
        Xml
    }

    /// <summary>
    /// İçe aktarma formatı
    /// </summary>
    public enum DepartmentImportFormat
    {
        Excel = 1,
        Csv = 2,
        Json = 3
    }
}

 