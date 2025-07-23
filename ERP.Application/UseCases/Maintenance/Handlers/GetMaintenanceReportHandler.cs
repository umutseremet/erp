//using AutoMapper;
//using ERP.Application.Common.Models;
//using ERP.Application.DTOs.Vehicle;
//using ERP.Application.UseCases.Maintenance.Queries;
//using ERP.Core.Entities;
//using ERP.Core.Enums;
//using ERP.Core.Interfaces;
//using MediatR;

//namespace ERP.Application.UseCases.Maintenance.Handlers
//{
//    public class GetMaintenanceReportHandler : IRequestHandler<GetMaintenanceReportQuery, Result<MaintenancePerformanceReportDto>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public GetMaintenanceReportHandler(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<Result<MaintenancePerformanceReportDto>> Handle(GetMaintenanceReportQuery request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var startDate = request.Filter.StartDate ?? DateTime.UtcNow.AddYears(-1);
//                var endDate = request.Filter.EndDate ?? DateTime.UtcNow;

//                var maintenances = await _unitOfWork.VehicleMaintenances.GetByDateRangeAsync(startDate, endDate);

//                if (request.Filter.VehicleIds?.Any() == true)
//                    maintenances = maintenances.Where(m => request.Filter.VehicleIds.Contains(m.VehicleId));

//                if (request.Filter.Type.HasValue)
//                    maintenances = maintenances.Where(m => m.Type == request.Filter.Type.Value);

//                var report = new MaintenancePerformanceReportDto
//                {
//                    KPIs = CalculateKPIs(maintenances),
//                    VehiclePerformances = CalculateVehiclePerformances(maintenances),
//                    ProviderPerformances = CalculateProviderPerformances(maintenances),
//                    Efficiency = CalculateEfficiency(maintenances)
//                };

//                return Result<MaintenancePerformanceReportDto>.Success(report);
//            }
//            catch (Exception ex)
//            {
//                return Result<MaintenancePerformanceReportDto>.Failure($"Bakım performans raporu oluşturulurken hata: {ex.Message}");
//            }
//        }

//        private MaintenanceKPIDto CalculateKPIs(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            var completed = maintenances.Where(m => m.IsCompleted);
//            var overdue = maintenances.Where(m => !m.IsCompleted && m.ScheduledDate < DateTime.UtcNow);

//            return new MaintenanceKPIDto
//            {
//                MTBF = CalculateMTBF(maintenances),
//                MTTR = CalculateMTTR(completed),
//                ScheduleCompliance = CalculateScheduleCompliance(maintenances),
//                CostVariance = CalculateCostVariance(completed),
//                VehicleAvailability = CalculateVehicleAvailability(maintenances),
//                MaintenanceEfficiency = CalculateMaintenanceEfficiency(maintenances)
//            };
//        }

//        private decimal CalculateMTBF(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            // Mean Time Between Failures calculation
//            var vehicles = maintenances.GroupBy(m => m.VehicleId);
//            var totalOperatingTime = 0m;
//            var totalFailures = 0;

//            foreach (var vehicleGroup in vehicles)
//            {
//                var vehicleMaintenances = vehicleGroup.OrderBy(m => m.ScheduledDate).ToList();
//                if (vehicleMaintenances.Count > 1)
//                {
//                    for (int i = 1; i < vehicleMaintenances.Count; i++)
//                    {
//                        totalOperatingTime += (decimal)(vehicleMaintenances[i].ScheduledDate - vehicleMaintenances[i - 1].ScheduledDate).TotalDays;
//                        totalFailures++;
//                    }
//                }
//            }

//            return totalFailures > 0 ? totalOperatingTime / totalFailures : 0;
//        }

//        private decimal CalculateMTTR(IEnumerable<VehicleMaintenance> completedMaintenances)
//        {
//            // Mean Time To Repair calculation
//            var repairTimes = completedMaintenances
//                .Where(m => m.CompletedDate.HasValue)
//                .Select(m => (decimal)(m.CompletedDate.Value - m.ScheduledDate).TotalHours)
//                .Where(t => t > 0);

//            return repairTimes.Any() ? repairTimes.Average() : 0;
//        }

//        private decimal CalculateScheduleCompliance(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            var total = maintenances.Count();
//            if (total == 0) return 100;

//            var onTime = maintenances.Count(m =>
//                !m.IsCompleted ||
//                (m.CompletedDate.HasValue && m.CompletedDate.Value <= m.ScheduledDate.AddDays(1)));

//            return (decimal)onTime / total * 100;
//        }

//        private decimal CalculateCostVariance(IEnumerable<VehicleMaintenance> completedMaintenances)
//        {
//            // Cost variance calculation (placeholder)
//            return 5.2m; // Example: 5.2% variance
//        }

//        private decimal CalculateVehicleAvailability(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            // Vehicle availability calculation (placeholder)
//            return 92.5m; // Example: 92.5% availability
//        }

//        private decimal CalculateMaintenanceEfficiency(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            // Overall maintenance efficiency (placeholder)
//            return 87.3m; // Example: 87.3% efficiency
//        }

//        private List<VehiclePerformanceDto> CalculateVehiclePerformances(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            return maintenances
//                .GroupBy(m => new { m.VehicleId, m.Vehicle.PlateNumber })
//                .Select(g => new VehiclePerformanceDto
//                {
//                    VehicleId = g.Key.VehicleId,
//                    PlateNumber = g.Key.PlateNumber,
//                    Reliability = CalculateReliability(g),
//                    MaintenanceCostRatio = CalculateMaintenanceCostRatio(g),
//                    BreakdownCount = g.Count(m => m.Type == MaintenanceType.Emergency),
//                    AvailabilityRate = CalculateAvailabilityRate(g),
//                    PerformanceGrade = CalculatePerformanceGrade(g)
//                })
//                .OrderByDescending(v => v.Reliability)
//                .ToList();
//        }

//        private decimal CalculateReliability(IGrouping<object, VehicleMaintenance> vehicleMaintenances)
//        {
//            var emergencyCount = vehicleMaintenances.Count(m => m.Type == MaintenanceType.Emergency);
//            var totalCount = vehicleMaintenances.Count();
//            return totalCount > 0 ? (decimal)(totalCount - emergencyCount) / totalCount * 100 : 100;
//        }

//        private decimal CalculateMaintenanceCostRatio(IGrouping<object, VehicleMaintenance> vehicleMaintenances)
//        {
//            var totalCost = vehicleMaintenances.Sum(m => m.Cost ?? 0);
//            // This would need vehicle value to calculate proper ratio
//            return totalCost / 100000m * 100; // Placeholder calculation
//        }

//        private decimal CalculateAvailabilityRate(IGrouping<object, VehicleMaintenance> vehicleMaintenances)
//        {
//            // Calculate based on maintenance downtime
//            return 95.0m; // Placeholder
//        }

//        private string CalculatePerformanceGrade(IGrouping<object, VehicleMaintenance> vehicleMaintenances)
//        {
//            var reliability = CalculateReliability(vehicleMaintenances);
//            return reliability switch
//            {
//                >= 95 => "A",
//                >= 85 => "B",
//                >= 75 => "C",
//                >= 65 => "D",
//                _ => "F"
//            };
//        }

//        private List<ServiceProviderPerformanceDto> CalculateProviderPerformances(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            return maintenances
//                .Where(m => !string.IsNullOrEmpty(m.ServiceProvider))
//                .GroupBy(m => m.ServiceProvider)
//                .Select(g => new ServiceProviderPerformanceDto
//                {
//                    ProviderName = g.Key!,
//                    OnTimeDeliveryRate = CalculateOnTimeDelivery(g),
//                    QualityRating = CalculateQualityRating(g),
//                    CostCompetitiveness = CalculateCostCompetitiveness(g),
//                    TotalJobs = g.Count(),
//                    CustomerSatisfaction = CalculateCustomerSatisfaction(g),
//                    OverallRating = CalculateOverallRating(g)
//                })
//                .OrderByDescending(p => p.OnTimeDeliveryRate)
//                .ToList();
//        }

//        private decimal CalculateOnTimeDelivery(IGrouping<string, VehicleMaintenance> providerMaintenances)
//        {
//            var completed = providerMaintenances.Where(m => m.IsCompleted && m.CompletedDate.HasValue);
//            var total = completed.Count();
//            if (total == 0) return 0;

//            var onTime = completed.Count(m => m.CompletedDate.Value <= m.ScheduledDate.AddDays(1));
//            return (decimal)onTime / total * 100;
//        }

//        private decimal CalculateQualityRating(IGrouping<string, VehicleMaintenance> providerMaintenances)
//        {
//            // Quality rating calculation (placeholder)
//            return 85.0m;
//        }

//        private decimal CalculateCostCompetitiveness(IGrouping<string, VehicleMaintenance> providerMaintenances)
//        {
//            // Cost competitiveness calculation (placeholder)
//            return 78.5m;
//        }

//        private decimal CalculateCustomerSatisfaction(IGrouping<string, VehicleMaintenance> providerMaintenances)
//        {
//            // Customer satisfaction calculation (placeholder)
//            return 88.2m;
//        }

//        private string CalculateOverallRating(IGrouping<string, VehicleMaintenance> providerMaintenances)
//        {
//            var onTime = CalculateOnTimeDelivery(providerMaintenances);
//            return onTime switch
//            {
//                >= 90 => "Excellent",
//                >= 80 => "Good",
//                >= 70 => "Average",
//                _ => "Poor"
//            };
//        }

//        private MaintenanceEfficiencyDto CalculateEfficiency(IEnumerable<VehicleMaintenance> maintenances)
//        {
//            return new MaintenanceEfficiencyDto
//            {
//                PlannedVsActualRatio = 92.3m,
//                ResourceUtilization = 87.5m,
//                CostEfficiency = 89.1m,
//                TimeEfficiency = 91.2m,
//                OverallEfficiency = 90.0m,
//                ImprovementAreas = new List<EfficiencyImprovementDto>
//                {
//                    new EfficiencyImprovementDto
//                    {
//                        Area = "Malzeme Tedarik Süresi",
//                        CurrentScore = 75.0m,
//                        TargetScore = 90.0m,
//                        Recommendation = "Tedarikçi anlaşmaları optimize edilmeli",
//                        PotentialSaving = 15000m
//                    }
//                }
//            };
//        }
//    }
//}