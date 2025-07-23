using ERP.Application.Common.Models;
using ERP.Application.DTOs.FuelTransaction;
using MediatR;

namespace ERP.Application.UseCases.Fuel.Commands
{
    public class UpdateFuelTransactionCommand : IRequest<Result<FuelTransactionDto>>
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string? StationName { get; set; }
        public string? StationAddress { get; set; }
        public decimal VehicleKm { get; set; }
        public string? Notes { get; set; }
    }
}
