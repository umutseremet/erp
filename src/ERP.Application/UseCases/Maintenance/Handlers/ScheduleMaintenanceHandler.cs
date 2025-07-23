using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using ERP.Application.Interfaces.Services;
using ERP.Application.UseCases.Maintenance.Commands;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Handlers
{
    public class ScheduleMaintenanceHandler : IRequestHandler<ScheduleMaintenanceCommand, Result<MaintenanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ScheduleMaintenanceHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<Result<MaintenanceDto>> Handle(ScheduleMaintenanceCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
            {
                return Result<MaintenanceDto>.Failure("Araç bulunamadı.");
            }

            var maintenanceType = Enum.Parse<ERP.Core.Enums.MaintenanceType>(request.Type);

            var maintenance = new ERP.Core.Entities.VehicleMaintenance(
                request.VehicleId,
                maintenanceType,
                request.ScheduledDate,
                request.VehicleKm ?? vehicle.CurrentKm,
                request.Description,
                request.ServiceProvider
            );

            // Calculate next maintenance based on intervals
            if (request.IntervalDays.HasValue || request.IntervalKm.HasValue)
            {
                var nextMaintenanceDate = request.IntervalDays.HasValue
                    ? request.ScheduledDate.AddDays(request.IntervalDays.Value)
                    : (DateTime?)null;

                var nextMaintenanceKm = request.IntervalKm.HasValue
                    ? (request.VehicleKm ?? vehicle.CurrentKm) + request.IntervalKm.Value
                    : (decimal?)null;

                maintenance.ScheduleNextMaintenance(nextMaintenanceDate, nextMaintenanceKm);
            }

            await _unitOfWork.VehicleMaintenances.AddAsync(maintenance);
            await _unitOfWork.SaveChangesAsync();

            // Create reminder notification if requested
            if (request.CreateReminder)
            {
                var reminderDate = request.ScheduledDate.AddDays(-request.ReminderDaysBefore);
                await _notificationService.ScheduleMaintenanceReminderAsync(maintenance.Id, reminderDate);
            }

            var dto = _mapper.Map<MaintenanceDto>(maintenance);
            return Result<MaintenanceDto>.Success(dto);
        }
    }
}