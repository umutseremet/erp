using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using ERP.Core.Enums;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Commands
{
    public class CreateMaintenanceCommand : IRequest<Result<MaintenanceDto>>
    {
        public int VehicleId { get; set; }
        public MaintenanceType Type { get; set; }
        public DateTime ScheduledDate { get; set; }
        public decimal VehicleKm { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ServiceProvider { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string? Notes { get; set; }
    }
}