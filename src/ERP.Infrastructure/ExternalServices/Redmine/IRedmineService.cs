using ERP.Infrastructure.ExternalServices.Redmine.Models;

namespace ERP.Infrastructure.ExternalServices.Redmine
{
    public interface IRedmineService
    {
        Task<RedmineUser?> GetUserAsync(int userId);
        Task<RedmineUser?> GetUserByEmailAsync(string email);
        Task<IEnumerable<RedmineUser>> GetUsersAsync();
        Task<RedmineIssue?> GetIssueAsync(int issueId);
        Task<RedmineIssue> CreateIssueAsync(RedmineIssue issue);
        Task<RedmineIssue> UpdateIssueAsync(RedmineIssue issue);
        Task<bool> DeleteIssueAsync(int issueId);
        Task<IEnumerable<RedmineProject>> GetProjectsAsync();
        Task<bool> IsUserActiveAsync(int userId);
        Task<IEnumerable<string>> GetUserRolesAsync(int userId);
    }
}