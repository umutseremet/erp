//using AutoMapper;
//using ERP.Application.Common.Models;
//using ERP.Application.Interfaces.Services;
//using ERP.Application.UseCases.Fuel.Queries;
//using ERP.Core.Entities;
//using ERP.Core.Interfaces;
//using MediatR;

//namespace ERP.Application.UseCases.Fuel.Handlers
//{
//    public class GetFuelConsumptionAnalysisHandler : IRequestHandler<GetFuelConsumptionAnalysisQuery, Result<FuelConsumptionAnalysisDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public GetFuelConsumptionAnalysisHandler(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        //public async Task<Result<FuelConsumptionAnalysisDto>> Handle(GetFuelConsumptionAnalysisQuery request, CancellationToken cancellationToken)
//        //{
//        //    try
//        //    {
//        //        var startDate = DateTime.UtcNow.AddMonths(-request.Months);
//        //        var transactions = await _unitOfWork.FuelTransactions.GetByVehicleAndDateRangeAsync(request.VehicleId, startDate, DateTime.UtcNow);

//        //        var monthlyData = transactions
//        //            .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
//        //            .Select(g => new MonthlyConsumptionDto
//        //            {
//        //                Year = g.Key.Year,
//        //                Month = g.Key.Month,
//        //                Consumption = CalculateConsumption(g.ToList()),
//        //                Cost = g.Sum(t => t.TotalAmount),
//        //                Distance = CalculateDistance(g.ToList()),
//        //                TransactionCount = g.Count()
//        //            })
//        //            .OrderBy(m => m.Year).ThenBy(m => m.Month)
//        //            .ToList();

//        //        var analysis = new FuelConsumptionAnalysisDto
//        //        {
//        //            VehicleId = request.VehicleId,
//        //            MonthlyData = monthlyData,
//        //            AverageMonthlyConsumption = monthlyData.Any() ? monthlyData.Average(m => m.Consumption) : 0,
//        //            TrendDirection = CalculateTrend(monthlyData),
//        //            EfficiencyScore = CalculateEfficiencyScore(monthlyData)
//        //        };

//        //        return Result<FuelConsumptionAnalysisDto>.Success(analysis);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return Result<FuelConsumptionAnalysisDto>.Failure($"Yakıt analizi hesaplanırken hata: {ex.Message}");
//        //    }
//        //}

//        private decimal CalculateConsumption(List<FuelTransaction> transactions)
//        {
//            if (transactions.Count < 2) return 0;

//            var totalFuel = transactions.Sum(t => t.Quantity);
//            var totalDistance = transactions.Max(t => t.VehicleKm) - transactions.Min(t => t.VehicleKm);

//            return totalDistance > 0 ? (totalFuel / totalDistance) * 100 : 0;
//        }

//        private decimal CalculateDistance(List<FuelTransaction> transactions)
//        {
//            if (transactions.Count < 2) return 0;
//            return transactions.Max(t => t.VehicleKm) - transactions.Min(t => t.VehicleKm);
//        }

//        //private decimal CalculateTrend(List<MonthlyConsumptionDto> data)
//        //{
//        //    if (data.Count < 2) return 0;

//        //    var recent = data.TakeLast(3).Average(d => d.Consumption);
//        //    var previous = data.Take(3).Average(d => d.Consumption);

//        //    return recent - previous;
//        //}

//        //private decimal CalculateEfficiencyScore(List<MonthlyConsumptionDto> data)
//        //{
//        //    if (!data.Any()) return 0;

//        //    var avgConsumption = data.Average(d => d.Consumption);
//        //    var standardConsumption = 8.0m; // Example standard

//        //    return Math.Max(0, Math.Min(100, (standardConsumption / avgConsumption) * 100));
//        //}
//    }
//}