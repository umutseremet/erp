using ERP.Application.UseCases.Fuel.Commands;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using FluentValidation;

namespace ERP.Application.Validators.FuelTransaction
{
    public class CreateFuelTransactionValidator : AbstractValidator<CreateFuelTransactionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFuelTransactionValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.VehicleId)
                .GreaterThan(0).WithMessage("Geçerli bir araç ID'si giriniz")
                .MustAsync(VehicleExists).WithMessage("Araç bulunamadı");

            RuleFor(x => x.FuelCardId)
                .MustAsync(FuelCardExistsAndActive).WithMessage("Geçersiz yakıt kartı")
                .When(x => x.FuelCardId.HasValue);

            RuleFor(x => x.TransactionDate)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("İşlem tarihi gelecekte olamaz")
                .GreaterThan(DateTime.Now.AddYears(-2)).WithMessage("İşlem tarihi çok eskide olamaz");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Yakıt miktarı pozitif olmalıdır")
                .LessThan(1000).WithMessage("Yakıt miktarı 1000 litreden az olmalıdır");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Birim fiyat pozitif olmalıdır")
                .LessThan(1000).WithMessage("Birim fiyat çok yüksek");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("Toplam tutar pozitif olmalıdır")
                .Must((command, totalAmount) => Math.Abs(totalAmount - (command.Quantity * command.UnitPrice)) < 0.01m)
                .WithMessage("Toplam tutar, miktar x birim fiyat ile uyuşmuyor");

            RuleFor(x => x.FuelType)
                .IsInEnum().WithMessage("Geçerli bir yakıt türü seçiniz");

            RuleFor(x => x.VehicleKm)
                .GreaterThanOrEqualTo(0).WithMessage("Araç kilometresi negatif olamaz")
                .LessThan(9999999).WithMessage("Araç kilometresi çok yüksek")
                .MustAsync(BeValidKilometer).WithMessage("Araç kilometresi mevcut kilometreden düşük olamaz");

            RuleFor(x => x.StationName)
                .MaximumLength(200).WithMessage("İstasyon adı en fazla 200 karakter olabilir");

            RuleFor(x => x.StationAddress)
                .MaximumLength(500).WithMessage("İstasyon adresi en fazla 500 karakter olabilir");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notlar en fazla 1000 karakter olabilir");
        }

        private async Task<bool> VehicleExists(int vehicleId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Vehicles.ExistsAsync(vehicleId);
        }

        private async Task<bool> FuelCardExistsAndActive(int? fuelCardId, CancellationToken cancellationToken)
        {
            if (!fuelCardId.HasValue) return true;

            var fuelCard = await _unitOfWork.Repository<FuelCard>().GetByIdAsync(fuelCardId.Value);
            return fuelCard != null && fuelCard.IsActive && !fuelCard.IsExpired;
        }

        private async Task<bool> BeValidKilometer(CreateFuelTransactionCommand command, decimal vehicleKm, CancellationToken cancellationToken)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(command.VehicleId);
            return vehicle == null || vehicleKm >= vehicle.CurrentKm;
        }
    }
}