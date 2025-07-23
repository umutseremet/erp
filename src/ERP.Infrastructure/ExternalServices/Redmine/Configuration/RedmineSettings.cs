namespace ERP.Infrastructure.ExternalServices.Redmine.Configuration
{
    public class RedmineSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public bool EnableLogging { get; set; } = true;
        public string DefaultAssigneeId { get; set; } = string.Empty;
        public string DefaultTrackerId { get; set; } = "1";
        public string DefaultPriorityId { get; set; } = "2";
    }
}