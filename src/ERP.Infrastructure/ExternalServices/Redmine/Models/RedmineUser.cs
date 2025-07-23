using System.Text.Json.Serialization;

namespace ERP.Infrastructure.ExternalServices.Redmine.Models
{
    public class RedmineUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("firstname")]
        public string? FirstName { get; set; }

        [JsonPropertyName("lastname")]
        public string? LastName { get; set; }

        [JsonPropertyName("mail")]
        public string? Mail { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("last_login_on")]
        public DateTime? LastLoginOn { get; set; }

        [JsonPropertyName("groups")]
        public List<RedmineGroup>? Groups { get; set; }

        [JsonPropertyName("custom_fields")]
        public List<RedmineCustomField>? CustomFields { get; set; }
    }

    public class RedmineGroup
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class RedmineCustomField
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }
}