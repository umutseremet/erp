using ERP.Infrastructure.ExternalServices.Redmine.Configuration;
using ERP.Infrastructure.ExternalServices.Redmine.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ERP.Infrastructure.ExternalServices.Redmine
{
    public class RedmineService : IRedmineService
    {
        private readonly HttpClient _httpClient;
        private readonly RedmineSettings _settings;

        public RedmineService(HttpClient httpClient, IOptions<RedmineSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            // HttpClient konfigürasyonu
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("X-Redmine-API-Key", _settings.ApiKey);
            _httpClient.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);
        }

        public async Task<RedmineUser?> GetUserAsync(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/users/{userId}.json");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RedmineResponse<RedmineUser>>(json);

                return result?.User;
            }
            catch
            {
                return null;
            }
        }

        public async Task<RedmineUser?> GetUserByEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/users.json?name={email}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RedmineResponse<List<RedmineUser>>>(json);

                return result?.Users?.FirstOrDefault(u => u.Mail?.Equals(email, StringComparison.OrdinalIgnoreCase) == true);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<RedmineUser>> GetUsersAsync()
        {
            try
            {
                var allUsers = new List<RedmineUser>();
                var offset = 0;
                const int limit = 100;

                while (true)
                {
                    var response = await _httpClient.GetAsync($"/users.json?offset={offset}&limit={limit}");

                    if (!response.IsSuccessStatusCode)
                        break;

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<RedmineResponse<List<RedmineUser>>>(json);

                    if (result?.Users == null || !result.Users.Any())
                        break;

                    allUsers.AddRange(result.Users);

                    if (result.Users.Count < limit)
                        break;

                    offset += limit;
                }

                return allUsers;
            }
            catch
            {
                return new List<RedmineUser>();
            }
        }

        public async Task<RedmineIssue?> GetIssueAsync(int issueId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/issues/{issueId}.json");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RedmineResponse<RedmineIssue>>(json);

                return result?.Issue;
            }
            catch
            {
                return null;
            }
        }

        public async Task<RedmineIssue> CreateIssueAsync(RedmineIssue issue)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { issue });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/issues.json", content);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RedmineResponse<RedmineIssue>>(json);

                return result?.Issue ?? issue;
            }
            catch
            {
                throw;
            }
        }

        public async Task<RedmineIssue> UpdateIssueAsync(RedmineIssue issue)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(new { issue });
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/issues/{issue.Id}.json", content);
                response.EnsureSuccessStatusCode();

                return issue;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteIssueAsync(int issueId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/issues/{issueId}.json");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<RedmineProject>> GetProjectsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/projects.json");

                if (!response.IsSuccessStatusCode)
                    return new List<RedmineProject>();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RedmineResponse<List<RedmineProject>>>(json);

                return result?.Projects ?? new List<RedmineProject>();
            }
            catch
            {
                return new List<RedmineProject>();
            }
        }

        public async Task<bool> IsUserActiveAsync(int userId)
        {
            var user = await GetUserAsync(userId);
            return user?.Status == 1; // 1 = Active in Redmine
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            try
            {
                var user = await GetUserAsync(userId);
                return user?.Groups?.Select(g => g.Name) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}