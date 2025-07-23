using ERP.Application.DTOs.Redmine;

namespace ERP.Application.Interfaces.External
{
    /// <summary>
    /// Redmine API entegrasyonu için servis interface'i
    /// </summary>
    public interface IRedmineApiService
    {
        #region User Management

        /// <summary>
        /// Redmine'dan tüm kullanıcıları getirir
        /// </summary>
        Task<IEnumerable<RedmineUserDto>> GetAllUsersAsync();

        /// <summary>
        /// ID'ye göre kullanıcı getirir
        /// </summary>
        Task<RedmineUserDto?> GetUserByIdAsync(int userId);

        /// <summary>
        /// Email'e göre kullanıcı getirir
        /// </summary>
        Task<RedmineUserDto?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Aktif kullanıcıları getirir
        /// </summary>
        Task<IEnumerable<RedmineUserDto>> GetActiveUsersAsync();

        /// <summary>
        /// Kullanıcı oluşturur
        /// </summary>
        Task<RedmineUserDto?> CreateUserAsync(CreateRedmineUserDto createUser);

        /// <summary>
        /// Kullanıcı günceller
        /// </summary>
        Task<RedmineUserDto?> UpdateUserAsync(int userId, UpdateRedmineUserDto updateUser);

        /// <summary>
        /// Kullanıcıyı deaktive eder
        /// </summary>
        Task<bool> DeactivateUserAsync(int userId);

        /// <summary>
        /// Kullanıcı projelere atama durumunu kontrol eder
        /// </summary>
        Task<IEnumerable<RedmineProjectMembershipDto>> GetUserProjectMembershipsAsync(int userId);

        #endregion User Management

        #region Project Management

        /// <summary>
        /// Tüm projeleri getirir
        /// </summary>
        Task<IEnumerable<RedmineProjectDto>> GetAllProjectsAsync();

        /// <summary>
        /// ID'ye göre proje getirir
        /// </summary>
        Task<RedmineProjectDto?> GetProjectByIdAsync(int projectId);

        /// <summary>
        /// Aktif projeleri getirir
        /// </summary>
        Task<IEnumerable<RedmineProjectDto>> GetActiveProjectsAsync();

        /// <summary>
        /// Kullanıcının dahil olduğu projeleri getirir
        /// </summary>
        Task<IEnumerable<RedmineProjectDto>> GetUserProjectsAsync(int userId);

        /// <summary>
        /// Proje üyeliklerini getirir
        /// </summary>
        Task<IEnumerable<RedmineProjectMembershipDto>> GetProjectMembershipsAsync(int projectId);

        #endregion Project Management

        #region Issue Management

        /// <summary>
        /// Issue oluşturur
        /// </summary>
        Task<RedmineIssueDto?> CreateIssueAsync(CreateRedmineIssueDto createIssue);

        /// <summary>
        /// Issue günceller
        /// </summary>
        Task<RedmineIssueDto?> UpdateIssueAsync(int issueId, UpdateRedmineIssueDto updateIssue);

        /// <summary>
        /// Issue'yu kapatır
        /// </summary>
        Task<bool> CloseIssueAsync(int issueId, string? notes = null);

        /// <summary>
        /// ID'ye göre issue getirir
        /// </summary>
        Task<RedmineIssueDto?> GetIssueByIdAsync(int issueId);

        /// <summary>
        /// Kullanıcının issue'larını getirir
        /// </summary>
        Task<IEnumerable<RedmineIssueDto>> GetUserIssuesAsync(int userId, RedmineIssueFilter? filter = null);

        /// <summary>
        /// Projenin issue'larını getirir
        /// </summary>
        Task<IEnumerable<RedmineIssueDto>> GetProjectIssuesAsync(int projectId, RedmineIssueFilter? filter = null);

        /// <summary>
        /// Issue'ya not ekler
        /// </summary>
        Task<bool> AddIssueNoteAsync(int issueId, string notes, bool isPrivate = false);

        #endregion Issue Management

        #region Role and Permission Management

        /// <summary>
        /// Tüm rolleri getirir
        /// </summary>
        Task<IEnumerable<RedmineRoleDto>> GetAllRolesAsync();

        /// <summary>
        /// Kullanıcının rollerini getirir
        /// </summary>
        Task<IEnumerable<RedmineRoleDto>> GetUserRolesAsync(int userId);

        /// <summary>
        /// Kullanıcıya rol atar
        /// </summary>
        Task<bool> AssignUserToProjectAsync(int userId, int projectId, int roleId);

        /// <summary>
        /// Kullanıcının proje rolünü kaldırır
        /// </summary>
        Task<bool> RemoveUserFromProjectAsync(int userId, int projectId);

        #endregion Role and Permission Management

        #region Custom Fields

        /// <summary>
        /// Özel alanları getirir
        /// </summary>
        Task<IEnumerable<RedmineCustomFieldDto>> GetCustomFieldsAsync();

        /// <summary>
        /// Kullanıcı özel alanlarını günceller
        /// </summary>
        Task<bool> UpdateUserCustomFieldsAsync(int userId, Dictionary<string, object> customFields);

        #endregion Custom Fields

        #region Authentication and Connection

        /// <summary>
        /// API bağlantısını test eder
        /// </summary>
        Task<bool> TestConnectionAsync();

        /// <summary>
        /// API anahtarını doğrular
        /// </summary>
        Task<bool> ValidateApiKeyAsync(string apiKey);

        /// <summary>
        /// Mevcut kullanıcı bilgilerini getirir (API key sahibi)
        /// </summary>
        Task<RedmineUserDto?> GetCurrentUserAsync();

        #endregion Authentication and Connection

        #region Synchronization

        /// <summary>
        /// Son senkronizasyon tarihinden sonra değişen kullanıcıları getirir
        /// </summary>
        Task<IEnumerable<RedmineUserDto>> GetUpdatedUsersSinceAsync(DateTime lastSyncDate);

        /// <summary>
        /// Son senkronizasyon tarihinden sonra değişen projeleri getirir
        /// </summary>
        Task<IEnumerable<RedmineProjectDto>> GetUpdatedProjectsSinceAsync(DateTime lastSyncDate);

        /// <summary>
        /// Senkronizasyon durumunu getirir
        /// </summary>
        Task<RedmineSyncStatusDto> GetSyncStatusAsync();

        #endregion Synchronization
    }

    #region DTOs

    public class CreateRedmineUserDto
    {
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
    }

    public class UpdateRedmineUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
        public Dictionary<string, object>? CustomFields { get; set; }
    }

    public class RedmineProjectMembershipDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<RedmineRoleDto> Roles { get; set; } = new List<RedmineRoleDto>();
    }

    public class CreateRedmineIssueDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }
        public RedmineIssuePriority Priority { get; set; } = RedmineIssuePriority.Normal;
        public DateTime? DueDate { get; set; }
        public Dictionary<string, object> CustomFields { get; set; } = new Dictionary<string, object>();
    }

    public class UpdateRedmineIssueDto
    {
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public int? AssignedToId { get; set; }
        public RedmineIssueStatus? Status { get; set; }
        public RedmineIssuePriority? Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DoneRatio { get; set; }
        public Dictionary<string, object>? CustomFields { get; set; }
    }

    public class RedmineRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();
    }

    public class RedmineCustomFieldDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FieldFormat { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public List<string>? PossibleValues { get; set; }
    }

    public class RedmineIssueFilter
    {
        public RedmineIssueStatus? Status { get; set; }
        public RedmineIssuePriority? Priority { get; set; }
        public int? AssignedToId { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public DateTime? UpdatedAfter { get; set; }
        public DateTime? UpdatedBefore { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }

    public enum RedmineProjectStatus
    {
        Active = 1,
        Closed = 5,
        Archived = 9
    }

    public enum RedmineIssueStatus
    {
        New = 1,
        InProgress = 2,
        Resolved = 3,
        Feedback = 4,
        Closed = 5,
        Rejected = 6
    }

    public enum RedmineIssuePriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Urgent = 4,
        Immediate = 5
    }

    #endregion DTOs
}