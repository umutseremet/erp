using ERP.Application.DTOs.User;
using ERP.Application.Common.Models;
using ERP.Core.Enums;

namespace ERP.Application.Interfaces.Services
{

    public interface IUserService
    {
        Task<Result<UserDto>> GetUserByIdAsync(int id);
        Task<Result<UserDto>> GetUserByEmailAsync(string email);
        Task<Result<UserDto>> GetUserByRedmineIdAsync(int redmineUserId);
        Task<Result<UserDto>> GetUserByEmployeeNumberAsync(string employeeNumber);
        Task<PaginatedResult<UserDto>> GetUsersPagedAsync(UserFilterDto filter, int pageNumber, int pageSize);
        Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<Result<IEnumerable<UserDto>>> GetActiveUsersAsync();
        Task<Result<IEnumerable<UserDto>>> GetUsersByDepartmentAsync(int departmentId);
        Task<Result<IEnumerable<UserDto>>> GetUsersByRoleAsync(int roleId);
        Task<Result<UserDto>> CreateUserAsync(CreateUserDto dto);
        Task<Result<UserDto>> UpdateUserAsync(int id, UpdateUserDto dto);
        Task<Result<bool>> DeleteUserAsync(int id);
        Task<Result<bool>> ActivateUserAsync(int id);
        Task<Result<bool>> DeactivateUserAsync(int id);
        Task<Result<bool>> SuspendUserAsync(int id, string reason);
        Task<Result<bool>> UnsuspendUserAsync(int id);
        Task<Result<bool>> RemoveUserFromDepartmentAsync(int userId, int departmentId);
        Task<Result<IEnumerable<UserDto>>> GetUserDepartmentsAsync(int userId);
        Task<Result<bool>> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<Result<IEnumerable<UserDto>>> GetUserRolesAsync(int userId);
        Task<Result<IEnumerable<UserDto>>> GetUserPermissionsAsync(int userId);
        Task<Result<bool>> HasPermissionAsync(int userId, string permission);
        Task<Result<bool>> AssignVehicleToUserAsync(int userId, int vehicleId);
        Task<Result<bool>> UnassignVehicleFromUserAsync(int userId, int vehicleId);
        Task<Result<IEnumerable<UserDto>>> GetUserVehiclesAsync(int userId);
        Task<Result<bool>> UpdateLastLoginAsync(int userId);
        Task<Result<IEnumerable<UserDto>>> GetUserSessionsAsync(int userId);
        Task<Result<bool>> TerminateAllSessionsAsync(int userId);
        Task<Result<bool>> IsEmailUniqueAsync(string email, int? excludeUserId = null);
        Task<Result<bool>> IsEmployeeNumberUniqueAsync(string employeeNumber, int? excludeUserId = null);
        Task<Result<bool>> IsRedmineUserIdUniqueAsync(int redmineUserId, int? excludeUserId = null);
        Task<Result<UserStatisticsDto>> GetUserStatisticsAsync();
        Task<Result<IEnumerable<UserDto>>> GetOverdueUsersAsync();
        Task<Result<IEnumerable<UserDto>>> GetUserDepartmentDistributionAsync();
        Task<Result<IEnumerable<UserDto>>> GetUserRoleDistributionAsync();
        Task<Result<IEnumerable<UserDto>>> ExportUsersAsync(UserExportFilterDto filter);
        Task<Result<bool>> ImportUsersAsync(byte[] data, UserImportFormat format);
        Task<Result<string>> GetImportTemplateAsync(UserImportFormat format);
        Task<Result<IEnumerable<CreateUserDto>>> CreateMultipleUsersAsync(IEnumerable<CreateUserDto> users);
        Task<Result<IEnumerable<UpdateUserBulkDto>>> UpdateMultipleUsersAsync(IEnumerable<UpdateUserBulkDto> users);
    }


    #region DTOs
     
    /// <summary>
    /// Genel kullanıcı istatistikleri için DTO
    /// </summary>
    public class UserOverallStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
        public int SuspendedUsers { get; set; }
        public int UsersWithVehicles { get; set; }
        public int UsersWithoutDepartment { get; set; }
        public int UsersWithoutRole { get; set; }
        public decimal ActiveUserPercentage { get; set; }
        public DateTime? LastUserCreated { get; set; }
        public DateTime? LastUserLogin { get; set; }
        public Dictionary<UserStatus, int> StatusDistribution { get; set; } = new Dictionary<UserStatus, int>();
        //public List<UserDepartmentDistributionDto> DepartmentDistribution { get; set; } = new List<UserDepartmentDistributionDto>();
        public List<UserRoleDistributionDto> RoleDistribution { get; set; } = new List<UserRoleDistributionDto>();
    }

     

    /// <summary>
    /// Kullanıcı rol dağılımı için DTO
    /// </summary>
    public class UserRoleDistributionDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public decimal Percentage { get; set; }
        public int ActiveAssignments { get; set; }
        public int ExpiredAssignments { get; set; }
    }

    /// <summary>
    /// Kullanıcı oturumu için DTO
    /// </summary>
    public class UserSessionDto
    {
        public string SessionId { get; set; } = string.Empty;
        public DateTime LoginTime { get; set; }
        public DateTime? LastActivity { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
    }

    /// <summary>
    /// Kullanıcı denetim kaydı için DTO
    /// </summary>
    public class UserAuditLogDto
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string ActionBy { get; set; } = string.Empty;
    }

    

    

    /// <summary>
    /// İçe aktarma sonucu için DTO
    /// </summary>
    public class UserImportResult
    {
        public int TotalRecords { get; set; }
        public int SuccessfulImports { get; set; }
        public int FailedImports { get; set; }
        public int UpdatedRecords { get; set; }
        public int NewRecords { get; set; }
        public List<UserImportError> Errors { get; set; } = new List<UserImportError>();
        public List<UserImportWarning> Warnings { get; set; } = new List<UserImportWarning>();
        public bool IsSuccess => FailedImports == 0;
    }

    /// <summary>
    /// İçe aktarma hatası için DTO
    /// </summary>
    public class UserImportError
    {
        public int RowNumber { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string FieldValue { get; set; } = string.Empty;
    }

    /// <summary>
    /// İçe aktarma uyarısı için DTO
    /// </summary>
    public class UserImportWarning
    {
        public int RowNumber { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string WarningMessage { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
    }

     

    /// <summary>
    /// Dışa aktarma formatı
    /// </summary>
    public enum UserExportFormat
    {
        Excel = 1,
        Csv = 2,
        Pdf = 3,
        Json = 4
    }

    /// <summary>
    /// İçe aktarma formatı
    /// </summary>
    public enum UserImportFormat
    {
        Excel = 1,
        Csv = 2,
        Json = 3
    }

    #endregion
}