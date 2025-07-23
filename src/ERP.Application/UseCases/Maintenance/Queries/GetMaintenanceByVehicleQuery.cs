using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Queries
{
    public class GetMaintenanceByVehicleQuery : IRequest<Result<IEnumerable<MaintenanceDto>>>
    {
        public int VehicleId { get; set; }
        public bool IncludeCompleted { get; set; } = true;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}