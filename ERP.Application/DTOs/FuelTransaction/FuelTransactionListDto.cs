using ERP.Application.DTOs.Common;
using ERP.Core.Enums;

namespace ERP.Application.DTOs.FuelTransaction
{
    public class FuelTransactionListDto : BaseDto
    {
        public string VehiclePlateNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public string? StationName { get; set; }
        public decimal VehicleKm { get; set; }
    }
}