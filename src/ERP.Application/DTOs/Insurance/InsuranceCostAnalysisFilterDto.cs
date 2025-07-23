namespace ERP.Application.DTOs.Insurance
{
    public class InsuranceCostAnalysisFilterDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? VehicleId { get; set; }
        public string? InsuranceCompany { get; set; }
        public string? PolicyType { get; set; }
    }
}
