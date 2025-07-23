using ERP.Application.Common.Models;
using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.Vehicle.Handlers
{
    public class DeleteVehicleHandler : IRequestHandler<DeleteVehicleCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVehicleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.Id);
                if (vehicle == null)
                    return Result<bool>.Failure("Araç bulunamadı");

                if (vehicle.Status == VehicleStatus.Assigned)
                    return Result<bool>.Failure("Atanmış araç silinemez. Önce aracı geri alın.");

                // Check for fuel transactions
                var fuelTransactions = await _unitOfWork.FuelTransactions.GetByVehicleAsync(request.Id);
                if (fuelTransactions.Any())
                    return Result<bool>.Failure("Yakıt işlemleri bulunan araç silinemez.");

                // Check for maintenance records
                var maintenanceRecords = await _unitOfWork.VehicleMaintenances.GetByVehicleAsync(request.Id);
                if (maintenanceRecords.Any())
                    return Result<bool>.Failure("Bakım kayıtları bulunan araç silinemez.");

                // Soft delete
                vehicle.MarkAsDeleted();
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Araç silinirken hata: {ex.Message}");
            }
        }
    }
}