using FluentValidation;

namespace ERP.Application.Validators.Common
{
    public class PlateNumberValidator : AbstractValidator<string>
    {
        public PlateNumberValidator()
        {
            RuleFor(plateNumber => plateNumber)
                .NotEmpty().WithMessage("Plaka numarası boş olamaz")
                .Length(5, 10).WithMessage("Plaka numarası 5-10 karakter arasında olmalıdır")
                .Matches(@"^[0-9]{2}\s?[A-Z]{1,3}\s?[0-9]{1,4}$")
                .WithMessage("Türk plaka formatına uygun değil (örn: 34 ABC 1234)");
        }
    }
}