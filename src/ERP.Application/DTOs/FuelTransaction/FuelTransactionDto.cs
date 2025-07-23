// 1. DTOs/FuelTransaction/FuelTransactionDto.cs
using ERP.Application.DTOs.Common;
using ERP.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ERP.Application.DTOs.FuelTransaction
{
    public class FuelTransactionDto : BaseDto
    {
        public int VehicleId { get; set; }
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public int? FuelCardId { get; set; }
        public string? FuelCardNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public string? StationName { get; set; }
        public string? StationAddress { get; set; }
        public decimal VehicleKm { get; set; }
        public string? Notes { get; set; }
        public string Currency { get; set; } = "TRY";
    }
}