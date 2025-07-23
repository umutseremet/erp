using System.Text.Json.Serialization;

namespace ERP.Infrastructure.ExternalServices.Redmine.Models
{
    public class RedmineIssue
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("project")]
        public RedmineProject? Project { get; set; }

        [JsonPropertyName("tracker")]
        public RedmineTracker? Tracker { get; set; }

        [JsonPropertyName("status")]
        public RedmineStatus? Status { get; set; }

        [JsonPropertyName("priority")]
        public RedminePriority? Priority { get; set; }

        [JsonPropertyName("author")]
        public RedmineUser? Author { get; set; }

        [JsonPropertyName("assigned_to")]
        public RedmineUser? AssignedTo { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("due_date")]
        public DateTime? DueDate { get; set; }

        [JsonPropertyName("done_ratio")]
        public int DoneRatio { get; set; }

        [JsonPropertyName("created_on")]
        public DateTime? CreatedOn { get; set; }

        [JsonPropertyName("updated_on")]
        public DateTime? UpdatedOn { get; set; }

        [JsonPropertyName("closed_on")]
        public DateTime? ClosedOn { get; set; }

        [JsonPropertyName("custom_fields")]
        public List<RedmineCustomField>? CustomFields { get; set; }
    }

    public class RedmineTracker
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

    public class RedmineStatus
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("is_closed")]
        public bool IsClosed { get; set; }
    }

    public class RedminePriority
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }
}