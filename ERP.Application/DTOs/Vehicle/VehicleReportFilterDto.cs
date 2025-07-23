using ERP.Core.Enums;

namespace ERP.Application.DTOs.Vehicle
{
    public class VehicleReportFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? VehicleId { get; set; }
        public VehicleStatus? Status { get; set; }
        public VehicleType? Type { get; set; }
        public int? DepartmentId { get; set; }
    }
}
