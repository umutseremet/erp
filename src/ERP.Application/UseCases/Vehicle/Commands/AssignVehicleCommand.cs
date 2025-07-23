using ERP.Application.Common.Models;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Commands
{
    public class AssignVehicleCommand : IRequest<Result<bool>>
    {
        public int VehicleId { get; set; }
        public int UserId { get; set; }
        public DateTime? AssignmentDate { get; set; }
        public string? Notes { get; set; }
    }
}