using FluentValidation;

namespace ERP.Application.Validators.Common
{
    public class PhoneNumberValidator : AbstractValidator<string>
    {
        public PhoneNumberValidator()
        {
            RuleFor(phoneNumber => phoneNumber)
                .Matches(@"^(\+90|0)?[5][0-9]{9}$")
                .WithMessage("Geçerli bir Türk telefon numarası giriniz (5XXXXXXXXX formatında)")
                .When(x => !string.IsNullOrEmpty(x));
        }
    }
}