//using AutoMapper;
//using ERP.Application.Common.Models;
//using ERP.Application.UseCases.Maintenance.Queries;
//using ERP.Core.Enums;
//using ERP.Core.Interfaces;
//using MediatR;

//namespace ERP.Application.UseCases.Maintenance.Handlers
//{
//    public class GetUpcomingMaintenanceHandler : IRequestHandler<GetUpcomingMaintenanceQuery, Result<IEnumerable<UpcomingMaintenanceDto>>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;

//        public GetUpcomingMaintenanceHandler(IUnitOfWork unitOfWork, IMapper mapper)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//        }

//        public async Task<Result<IEnumerable<UpcomingMaintenanceDto>>> Handle(GetUpcomingMaintenanceQuery request, CancellationToken cancellationToken)
//        {
//            try
//            {
//                var upcomingDate = DateTime.UtcNow.AddDays(request.DaysAhead);
//                var maintenances = await _unitOfWork.VehicleMaintenances.GetUpcomingMaintenanceAsync();

//                var filteredMaintenances = maintenances.Where(m =>
//                    m.ScheduledDate <= upcomingDate &&
//                    !m.IsCompleted);

//                if (request.VehicleId.HasValue)
//                    filteredMaintenances = filteredMaintenances.Where(m => m.VehicleId == request.VehicleId.Value);

//                if (request.Type.HasValue)
//                    filteredMaintenances = filteredMaintenances.Where(m => m.Type == request.Type.Value);

//                var upcomingMaintenanceDtos = filteredMaintenances.Select(m => new UpcomingMaintenanceDto
//                {
//                    Id = m.Id,
//                    VehicleId = m.VehicleId,
//                    VehiclePlateNumber = m.Vehicle.PlateNumber,
//                    VehicleInfo = $"{m.Vehicle.Year} {m.Vehicle.Brand} {m.Vehicle.Model}",
//                    Type = m.Type,
//                    ScheduledDate = m.ScheduledDate,
//                    Description = m.Description,
//                    DaysUntilDue = (m.ScheduledDate - DateTime.UtcNow).Days,
//                    ServiceProvider = m.ServiceProvider,
//                    EstimatedCost = m.Cost,
//                    PriorityLevel = DeterminePriorityLevel(m.ScheduledDate, m.Type)
//                }).OrderBy(m => m.ScheduledDate);

//                return Result<IEnumerable<UpcomingMaintenanceDto>>.Success(upcomingMaintenanceDtos);
//            }
//            catch (Exception ex)
//            {
//                return Result<IEnumerable<UpcomingMaintenanceDto>>.Failure($"Yaklaşan bakımlar getirilirken hata: {ex.Message}");
//            }
//        }

//        private static string DeterminePriorityLevel(DateTime scheduledDate, MaintenanceType type)
//        {
//            var daysUntilDue = (scheduledDate - DateTime.UtcNow).Days;

//            if (type == MaintenanceType.Emergency)
//                return "High";

//            return daysUntilDue switch
//            {
//                <= 0 => "High",
//                <= 7 => "Medium",
//                _ => "Low"
//            };
//        }
//    }
//}