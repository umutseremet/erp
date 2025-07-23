using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using ERP.Application.DTOs.Notification;
using ERP.Application.Interfaces.Services;
using ERP.Application.UseCases.Maintenance.Commands;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Handlers
{
    public class CompleteMaintenanceHandler : IRequestHandler<CompleteMaintenanceCommand, Result<MaintenanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CompleteMaintenanceHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<Result<MaintenanceDto>> Handle(CompleteMaintenanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maintenance = await _unitOfWork.VehicleMaintenances.GetByIdAsync(request.Id);
                if (maintenance == null)
                    return Result<MaintenanceDto>.Failure("Bakım kaydı bulunamadı");

                if (maintenance.IsCompleted)
                    return Result<MaintenanceDto>.Failure("Bakım zaten tamamlanmış");

                maintenance.Complete(request.CompletedDate, request.ActualCost, request.CompletionNotes);

                if (request.NextMaintenanceDate.HasValue || request.NextMaintenanceKm.HasValue)
                {
                    maintenance.ScheduleNextMaintenance(request.NextMaintenanceDate, request.NextMaintenanceKm);
                }

                await _unitOfWork.SaveChangesAsync();

                // Update vehicle status if it was in maintenance
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(maintenance.VehicleId);
                if (vehicle?.Status == VehicleStatus.InMaintenance)
                {
                    vehicle.SetStatus(VehicleStatus.Available);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Send completion notification
                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                {
                    VehicleId = maintenance.VehicleId,
                    UserId = vehicle?.AssignedUserId,
                    Type = NotificationType.ServiceCompleted,
                    Title = "Bakım Tamamlandı",
                    Message = $"{vehicle?.PlateNumber} plakalı aracın {maintenance.Type} bakımı tamamlandı",
                    ScheduledDate = DateTime.UtcNow,
                    Priority = 1
                });

                var dto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım tamamlanırken hata: {ex.Message}");
            }
        }
    }
}