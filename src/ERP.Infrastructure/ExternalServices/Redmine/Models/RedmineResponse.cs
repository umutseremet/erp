using System.Text.Json.Serialization;

namespace ERP.Infrastructure.ExternalServices.Redmine.Models
{
    public class RedmineResponse<T>
    {
        [JsonPropertyName("user")]
        public RedmineUser? User { get; set; }

        [JsonPropertyName("users")]
        public List<RedmineUser>? Users { get; set; }

        [JsonPropertyName("issue")]
        public RedmineIssue? Issue { get; set; }

        [JsonPropertyName("issues")]
        public List<RedmineIssue>? Issues { get; set; }

        [JsonPropertyName("project")]
        public RedmineProject? Project { get; set; }

        [JsonPropertyName("projects")]
        public List<RedmineProject>? Projects { get; set; }

        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }
    }
}