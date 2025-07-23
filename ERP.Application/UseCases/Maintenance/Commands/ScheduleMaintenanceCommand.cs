using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Commands
{
    public class ScheduleMaintenanceCommand : IRequest<Result<MaintenanceDto>>
    {
        public int VehicleId { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public decimal? VehicleKm { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? ServiceProvider { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string? Priority { get; set; } = "Medium";
        public string? Notes { get; set; }
        public int? IntervalDays { get; set; }
        public decimal? IntervalKm { get; set; }
        public bool CreateReminder { get; set; } = true;
        public int ReminderDaysBefore { get; set; } = 7;
    }
}