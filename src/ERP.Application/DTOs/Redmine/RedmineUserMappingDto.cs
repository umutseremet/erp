namespace ERP.Application.DTOs.Redmine
{
    public class RedmineUserMappingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;
        public int RedmineUserId { get; set; }
        public string RedmineLogin { get; set; } = string.Empty;
        public string RedmineFullName { get; set; } = string.Empty;
        public DateTime MappedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastSyncDate { get; set; }
        public string? SyncStatus { get; set; }
    }
}