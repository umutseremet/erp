using ERP.Core.Enums;

namespace ERP.Application.DTOs.FuelTransaction
{
    public class FuelTransactionFilterDto
    {
        public int? VehicleId { get; set; }
        public int? FuelCardId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public FuelType? FuelType { get; set; }
        public string? StationName { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
    }
}
