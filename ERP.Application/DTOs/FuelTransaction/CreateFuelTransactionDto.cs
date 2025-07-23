using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.FuelTransaction
{
    public class CreateFuelTransactionDto
    {
        public int VehicleId { get; set; }
        public int? FuelCardId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public FuelType FuelType { get; set; }
        public string? StationName { get; set; }
        public string? StationAddress { get; set; }
        public decimal VehicleKm { get; set; }
        public string? Notes { get; set; }
    }
}