namespace ERP.Application.DTOs.Insurance
{
    public class InsuranceExportFilterDto
    {
        public int? VehicleId { get; set; }
        public string? InsuranceCompany { get; set; }
        public bool? IsActive { get; set; }
        public InsuranceExportFormat Format { get; set; } = InsuranceExportFormat.Excel;
    }

    public enum InsuranceImportFormat
    {
        Excel,
        Csv
    }

    public enum InsuranceExportFormat
    {
        Excel,
        Csv,
        Pdf
    }
}