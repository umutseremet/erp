using MediatR;
using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Application.Common.Models;
using ERP.Core.Interfaces;
using ERP.Application.Interfaces.Services;
using ERP.Application.DTOs.Notification;
using ERP.Core.Enums;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class AssignVehicleHandler : IRequestHandler<AssignVehicleCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public AssignVehicleHandler(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<Result<bool>> Handle(AssignVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.VehicleId);
                if (vehicle == null)
                    return Result<bool>.Failure("Araç bulunamadı");

                var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                if (vehicle.Status != VehicleStatus.Available)
                    return Result<bool>.Failure("Araç atama için uygun değil");

                vehicle.AssignToUser(request.UserId);
                await _unitOfWork.SaveChangesAsync();

                // Send notification
                await _notificationService.CreateNotificationAsync(new CreateNotificationDto
                {
                    VehicleId = request.VehicleId,
                    UserId = request.UserId,
                    Type = NotificationType.VehicleAssigned,
                    Title = "Araç Atandı",
                    Message = $"{vehicle.PlateNumber} plakalı araç size atandı",
                    ScheduledDate = DateTime.UtcNow,
                    Priority = 2
                });

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç atama sırasında hata: {ex.Message}");
            }
        }
    }
}