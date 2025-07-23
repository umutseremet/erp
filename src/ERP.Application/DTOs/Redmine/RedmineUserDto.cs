namespace ERP.Application.DTOs.Redmine
{
    public class RedmineUserDto
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class RedmineProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsPublic { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    public class RedmineIssueDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int TrackerId { get; set; }
        public string TrackerName { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int PriorityId { get; set; }
        public string PriorityName { get; set; } = string.Empty;
        public int? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DoneRatio { get; set; }
        public decimal? EstimatedHours { get; set; }
        public decimal? SpentHours { get; set; }
    }

    public class RedmineSyncStatusDto
    {
        public DateTime? LastUserSync { get; set; }
        public DateTime? LastProjectSync { get; set; }
        public int TotalSyncedUsers { get; set; }
        public int TotalSyncedProjects { get; set; }
        public bool IsApiConnected { get; set; }
        public string? LastSyncError { get; set; }
        public bool IsSyncInProgress { get; set; }
        public DateTime? NextScheduledSync { get; set; }
    }

    public class RedmineSyncScheduleDto
    {
        public bool EnableAutoSync { get; set; }
        public int SyncIntervalHours { get; set; } = 24;
        public TimeSpan SyncTime { get; set; } = new TimeSpan(2, 0, 0); // 02:00 AM
        public bool SyncUsers { get; set; } = true;
        public bool SyncProjects { get; set; } = true;
        public bool NotifyOnFailure { get; set; } = true;
        public List<string> NotificationEmails { get; set; } = new();
    }
}