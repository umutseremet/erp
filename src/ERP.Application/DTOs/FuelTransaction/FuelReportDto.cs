using ERP.Core.Enums;

namespace ERP.Application.DTOs.FuelTransaction
{
    public class FuelReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal AverageConsumption { get; set; }
        public List<FuelTransactionDto> Transactions { get; set; } = new();
        public Dictionary<string, decimal> CostByFuelType { get; set; } = new();
        public Dictionary<string, decimal> ConsumptionByVehicle { get; set; } = new();
    }
}