using ERP.Application.UseCases.Vehicle.Commands;
using ERP.Core.Interfaces;
using FluentValidation;

namespace ERP.Application.Validators.Vehicle
{
    public class UpdateVehicleValidator : AbstractValidator<UpdateVehicleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVehicleValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir araç ID'si giriniz");

            RuleFor(x => x.PlateNumber)
                .NotEmpty().WithMessage("Plaka numarası zorunludur")
                .Length(2, 20).WithMessage("Plaka numarası 2-20 karakter arasında olmalıdır")
                .Matches(@"^[0-9]{2}\s?[A-Z]{1,3}\s?[0-9]{1,4}$").WithMessage("Geçersiz plaka formatı")
                .MustAsync(BeUniquePlateNumber).WithMessage("Bu plaka numarası zaten kullanımda");

            RuleFor(x => x.VinNumber)
                .NotEmpty().WithMessage("VIN numarası zorunludur")
                .Length(17).WithMessage("VIN numarası 17 karakter olmalıdır")
                .Matches(@"^[A-HJ-NPR-Z0-9]{17}$").WithMessage("Geçersiz VIN formatı")
                .MustAsync(BeUniqueVinNumber).WithMessage("Bu VIN numarası zaten kullanımda");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Marka zorunludur")
                .Length(1, 50).WithMessage("Marka 1-50 karakter arasında olmalıdır");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model zorunludur")
                .Length(1, 50).WithMessage("Model 1-50 karakter arasında olmalıdır");

            RuleFor(x => x.Year)
                .GreaterThan(1900).WithMessage("Yıl 1900'den büyük olmalıdır")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("Yıl gelecek yıldan büyük olamaz");

            RuleFor(x => x.CurrentKm)
                .GreaterThanOrEqualTo(0).WithMessage("Kilometre negatif olamaz")
                .LessThan(9999999).WithMessage("Kilometre değeri çok yüksek");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notlar en fazla 1000 karakter olabilir");
        }

        private async Task<bool> BeUniquePlateNumber(UpdateVehicleCommand command, string plateNumber, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.Vehicles.IsPlateNumberExistsAsync(plateNumber, command.Id);
        }

        private async Task<bool> BeUniqueVinNumber(UpdateVehicleCommand command, string vinNumber, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.Vehicles.IsVinNumberExistsAsync(vinNumber, command.Id);
        }
    }
}