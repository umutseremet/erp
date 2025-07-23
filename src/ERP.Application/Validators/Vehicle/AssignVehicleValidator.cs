using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using FluentValidation;

namespace ERP.Application.Validators.Vehicle
{
    public class AssignVehicleValidator : AbstractValidator<AssignVehicleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignVehicleValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Geçerli bir araç ID'si giriniz")
                .MustAsync(VehicleExists).WithMessage("Araç bulunamadı")
                .MustAsync(VehicleIsAvailable).WithMessage("Araç atama için uygun değil");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Geçerli bir kullanıcı ID'si giriniz")
                .MustAsync(UserExists).WithMessage("Kullanıcı bulunamadı")
                .MustAsync(UserIsActive).WithMessage("Kullanıcı aktif değil");

            RuleFor(x => x.AssignmentDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Atama tarihi bugünden ileri olamaz")
                .When(x => x.AssignmentDate.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notlar en fazla 500 karakter olabilir");
        }

        private async Task<bool> VehicleExists(int vehicleId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Vehicles.ExistsAsync(vehicleId);
        }

        private async Task<bool> VehicleIsAvailable(int vehicleId, CancellationToken cancellationToken)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
            return vehicle?.Status == VehicleStatus.Available;
        }

        private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Users.ExistsAsync(userId);
        }

        private async Task<bool> UserIsActive(int userId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            return user?.Status == UserStatus.Active;
        }
    }
}