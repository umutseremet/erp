using ERP.Application.Common.Models;
using ERP.Application.DTOs.FuelTransaction;
using ERP.Core.Enums;
using MediatR;

namespace ERP.Application.UseCases.Fuel.Commands
{
    public class CreateFuelTransactionCommand : IRequest<Result<FuelTransactionDto>>
    {
        public int VehicleId { get; set; }
        public int? FuelCardId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public FuelType FuelType { get; set; }
        public string? StationName { get; set; }
        public string? StationAddress { get; set; }
        public decimal VehicleKm { get; set; }
        public string? Notes { get; set; }
    }
}
