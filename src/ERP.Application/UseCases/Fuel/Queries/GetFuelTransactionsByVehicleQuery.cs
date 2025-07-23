using ERP.Application.Common.Models;
using ERP.Application.DTOs.FuelTransaction;
using MediatR;

namespace ERP.Application.UseCases.Fuel.Queries
{
    public class GetFuelTransactionsByVehicleQuery : IRequest<Result<IEnumerable<FuelTransactionDto>>>
    {
        public int VehicleId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}