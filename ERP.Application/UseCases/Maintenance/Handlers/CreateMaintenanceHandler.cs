using AutoMapper;
using ERP.Application.Common.Models;
using ERP.Application.DTOs.Maintenance;
using ERP.Application.DTOs.Notification;
using ERP.Application.Interfaces.Services;
using ERP.Application.UseCases.Maintenance.Commands;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Maintenance.Handlers
{
    public class CreateMaintenanceHandler : IRequestHandler<CreateMaintenanceCommand, Result<MaintenanceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CreateMaintenanceHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<Result<MaintenanceDto>> Handle(CreateMaintenanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
                if (vehicle == null)
                    return Result<MaintenanceDto>.Failure("Araç bulunamadı");

                var maintenance = new VehicleMaintenance(
                    request.VehicleId,
                    request.Type,
                    request.ScheduledDate,
                    request.VehicleKm,
                    request.Description,
                    request.ServiceProvider
                );

                if (!string.IsNullOrEmpty(request.Notes))
                    maintenance.SetNotes(request.Notes);

                await _unitOfWork.VehicleMaintenances.AddAsync(maintenance);
                await _unitOfWork.SaveChangesAsync();

                // Create reminder notification
                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                {
                    VehicleId = request.VehicleId,
                    Type = NotificationType.MaintenanceDue,
                    Title = "Bakım Planlandı",
                    Message = $"{vehicle.PlateNumber} plakalı araç için {request.Type} bakımı {request.ScheduledDate:dd.MM.yyyy} tarihinde planlandı",
                    ScheduledDate = request.ScheduledDate.AddDays(-3), // 3 gün önceden hatırlatma
                    Priority = 2
                });

                var dto = _mapper.Map<MaintenanceDto>(maintenance);
                return Result<MaintenanceDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return Result<MaintenanceDto>.Failure($"Bakım kaydı oluşturulurken hata: {ex.Message}");
            }
        }
    }
}