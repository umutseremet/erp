using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleExportFilterDto
    {
        public VehicleStatus? Status { get; set; }
        public VehicleType? Type { get; set; }
        public int? DepartmentId { get; set; }
        public bool IncludeMaintenanceHistory { get; set; }
        public bool IncludeFuelHistory { get; set; }
        public VehicleExportFormat Format { get; set; } = VehicleExportFormat.Excel;
    }
}
