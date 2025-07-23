namespace ERP.Application.DTOs.Insurance
{
    public class InsuranceFilterDto
    {
        public int? VehicleId { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? PolicyType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsExpiring { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ExpiringDays { get; set; } = 30;
    }
}
