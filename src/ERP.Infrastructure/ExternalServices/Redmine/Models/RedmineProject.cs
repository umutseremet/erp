using System.Text.Json.Serialization;

namespace ERP.Infrastructure.ExternalServices.Redmine.Models
{
    public class RedmineProject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("identifier")]
        public string Identifier { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("is_public")]
        public bool IsPublic { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        [JsonPropertyName("parent")]
        public RedmineProject? Parent { get; set; }

        [JsonPropertyName("custom_fields")]
        public List<RedmineCustomField>? CustomFields { get; set; }
    }
}