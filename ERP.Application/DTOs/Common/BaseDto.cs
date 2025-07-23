namespace ERP.Application.DTOs.Common
{
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int? RedmineIssueId { get; set; }
    }

    public class PaginationDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";

        public int Skip => (PageNumber - 1) * PageSize;
        public int Take => PageSize;
    }

    public class FilterDto
    {
        public string? SearchTerm { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Dictionary<string, object> AdditionalFilters { get; set; } = new();
    }
}