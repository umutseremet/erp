//using AutoMapper;
//using ERP.Application.Common.Models;
//using ERP.Application.Interfaces.Services;
//using ERP.Application.UseCases.Fuel.Queries;
//using ERP.Core.Entities;
//using ERP.Core.Interfaces;
//using MediatR;

//namespace ERP.Application.UseCases.Fuel.Handlers
//{
//    public class GetFuelReportHandler : IRequestHandler<GetFuelReportQuery, Result<FuelCostReportDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public GetFuelReportHandler(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<Result<FuelCostReportDto>> Handle(GetFuelReportQuery request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var startDate = request.Filter.StartDate ?? DateTime.UtcNow.AddYears(-1);
//                var endDate = request.Filter.EndDate ?? DateTime.UtcNow;

//                var transactions = await _unitOfWork.FuelTransactions.GetByDateRangeAsync(startDate, endDate);

//                if (request.Filter.VehicleIds?.Any() == true)
//                    transactions = transactions.Where(t => request.Filter.VehicleIds.Contains(t.VehicleId));

//                if (request.Filter.FuelType.HasValue)
//                    transactions = transactions.Where(t => t.FuelType == request.Filter.FuelType.Value);

//                var totalCost = transactions.Sum(t => t.TotalAmount);
//                var monthlyPeriod = ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;
//                var averageMonthlyCost = monthlyPeriod > 0 ? totalCost / monthlyPeriod : 0;

//                var vehicleCosts = transactions
//                    .GroupBy(t => new { t.VehicleId, t.Vehicle.PlateNumber })
//                    .Select(g => new VehicleCostSummaryDto
//                    {
//                        VehicleId = g.Key.VehicleId,
//                        PlateNumber = g.Key.PlateNumber,
//                        TotalCost = g.Sum(t => t.TotalAmount),
//                        TransactionCount = g.Count(),
//                        AverageConsumption = CalculateAverageConsumption(g.ToList())
//                    })
//                    .OrderByDescending(v => v.TotalCost)
//                    .ToList();

//                var monthlyCosts = transactions
//                    .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
//                    .Select(g => new MonthlyCostDto
//                    {
//                        Year = g.Key.Year,
//                        Month = g.Key.Month,
//                        Cost = g.Sum(t => t.TotalAmount),
//                        Quantity = g.Sum(t => t.Quantity),
//                        AveragePrice = g.Average(t => t.UnitPrice)
//                    })
//                    .OrderBy(m => m.Year).ThenBy(m => m.Month)
//                    .ToList();

//                var costByFuelType = transactions
//                    .GroupBy(t => t.FuelType)
//                    .ToDictionary(g => g.Key, g => g.Sum(t => t.TotalAmount));

//                var report = new FuelCostReportDto
//                {
//                    TotalCost = totalCost,
//                    AverageMonthlyCost = averageMonthlyCost,
//                    VehicleCosts = vehicleCosts,
//                    MonthlyCosts = monthlyCosts,
//                    CostByFuelType = costByFuelType
//                };

//                return Result<FuelCostReportDto>.Success(report);
//            }
//            catch (Exception ex)
//            {
//                return Result<FuelCostReportDto>.Failure($"Yakıt raporu oluşturulurken hata: {ex.Message}");
//            }
//        }

//        private decimal CalculateAverageConsumption(List<FuelTransaction> transactions)
//        {
//            if (transactions.Count < 2) return 0;

//            var totalFuel = transactions.Sum(t => t.Quantity);
//            var totalDistance = transactions.Max(t => t.VehicleKm) - transactions.Min(t => t.VehicleKm);

//            return totalDistance > 0 ? (totalFuel / totalDistance) * 100 : 0;
//        }
//    }
//}