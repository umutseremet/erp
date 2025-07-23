using ERP.Application.Common.Models;
using ERP.Application.DTOs.Notification;
using ERP.Application.Interfaces.Services;
using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class ReturnVehicleHandler : IRequestHandler<ReturnVehicleCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public ReturnVehicleHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<Result<bool>> Handle(ReturnVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
                if (vehicle == null)
                    return Result<bool>.Failure("Araç bulunamadı");

                if (vehicle.Status != VehicleStatus.Assigned)
                    return Result<bool>.Failure("Araç atanmış durumda değil");

                var assignedUserId = vehicle.AssignedUserId;

                if (request.CurrentKm.HasValue)
                    vehicle.UpdateKilometer(request.CurrentKm.Value);

                vehicle.ReturnFromUser();
                await _unitOfWork.SaveChangesAsync();

                // Send notification to user
                if (assignedUserId.HasValue)
                {
                    await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                    {
                        VehicleId = request.VehicleId,
                        UserId = assignedUserId.Value,
                        Type = NotificationType.VehicleReturned,
                        Title = "Araç Teslim Alındı",
                        Message = $"{vehicle.PlateNumber} plakalı araç teslim alındı",
                        ScheduledDate = DateTime.UtcNow,
                        Priority = 1
                    });
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç teslim alma sırasında hata: {ex.Message}");
            }
        }
    }
}
