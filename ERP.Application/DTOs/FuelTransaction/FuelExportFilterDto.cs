using ERP.Core.Enums;

namespace ERP.Application.DTOs.FuelTransaction
{
    public class FuelExportFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VehicleId { get; set; }
        public FuelType? FuelType { get; set; }
        public FuelExportFormat Format { get; set; } = FuelExportFormat.Excel;
    }
}
