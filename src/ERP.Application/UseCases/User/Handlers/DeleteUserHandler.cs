using ERP.Application.Common.Models;
using ERP.Application.UseCases.User.Commands;
using ERP.Core.Interfaces;
using MediatR;

namespace ERP.Application.UseCases.User.Handlers
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
                if (user == null)
                    return Result<bool>.Failure("Kullanıcı bulunamadı");

                // Check if user has assigned vehicles
                var assignedVehicles = await _unitOfWork.Vehicles.GetVehiclesByUserAsync(request.Id);
                if (assignedVehicles.Any())
                    return Result<bool>.Failure("Kullanıcının atanmış araçları bulunmaktadır. Önce araçları geri alın.");

                // Soft delete
                user.MarkAsDeleted();
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Kullanıcı silinirken hata: {ex.Message}");
            }
        }
    }
}