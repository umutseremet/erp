using ERP.Application.Common.Models;
using ERP.Application.DTOs.Redmine;
using ERP.Application.Interfaces.External;

namespace ERP.Application.Interfaces.Services
{
    public interface IRedmineIntegrationService
    {
        Task<Result<bool>> SyncUsersFromRedmineAsync();
        Task<Result<bool>> SyncUserToRedmineAsync(int userId);
        Task<Result<bool>> SyncProjectsFromRedmineAsync();
        Task<Result<RedmineUserDto?>> GetRedmineUserAsync(int redmineUserId);
        Task<Result<IEnumerable<RedmineUserDto>>> GetAllRedmineUsersAsync();
        Task<Result<IEnumerable<RedmineProjectDto>>> GetAllRedmineProjectsAsync();
        Task<Result<bool>> CreateRedmineIssueAsync(CreateRedmineIssueDto issueDto);
        Task<Result<bool>> UpdateRedmineIssueAsync(int issueId, UpdateRedmineIssueDto updateDto);
        Task<Result<bool>> CloseRedmineIssueAsync(int issueId, string notes);
        Task<Result<IEnumerable<RedmineIssueDto>>> GetUserIssuesAsync(int userId);
        Task<Result<RedmineSyncStatusDto>> GetSyncStatusAsync();
        Task<Result<bool>> TestConnectionAsync();
        Task<Result<bool>> ValidateApiKeyAsync();
        Task<Result<RedmineUserMappingDto>> MapUserToRedmineAsync(int userId, int redmineUserId);
        Task<Result<IEnumerable<RedmineUserMappingDto>>> GetUserMappingsAsync();
        Task<Result<bool>> ScheduleSyncAsync(RedmineSyncScheduleDto schedule);
        Task<Result<RedmineSyncLogDto>> GetLastSyncLogAsync();
    }
}