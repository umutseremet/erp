using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Commands
{
    public class CompleteMaintenanceCommand : IRequest<Result<MaintenanceDto>>
    {
        public int Id { get; set; }
        public DateTime CompletedDate { get; set; }
        public decimal? ActualCost { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public decimal? NextMaintenanceKm { get; set; }
        public string? CompletionNotes { get; set; }
        public List<MaintenanceItemDto> Items { get; set; } = new List<MaintenanceItemDto>();
    }
}