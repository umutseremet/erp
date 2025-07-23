using ERP.Application.UseCases.User.Commands;
using ERP.Core.Interfaces;
using FluentValidation;

namespace ERP.Application.Validators.User
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.RedmineUserId)
                .GreaterThan(0).WithMessage("Geçerli bir Redmine kullanıcı ID'si giriniz")
                .MustAsync(BeUniqueRedmineUserId).WithMessage("Bu Redmine kullanıcı ID'si zaten kullanımda");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Ad zorunludur")
                .Length(1, 100).WithMessage("Ad 1-100 karakter arasında olmalıdır")
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$").WithMessage("Ad sadece harf içerebilir");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyad zorunludur")
                .Length(1, 100).WithMessage("Soyad 1-100 karakter arasında olmalıdır")
                .Matches(@"^[a-zA-ZğüşöçıİĞÜŞÖÇ\s]+$").WithMessage("Soyad sadece harf içerebilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email zorunludur")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .Length(1, 200).WithMessage("Email en fazla 200 karakter olabilir")
                .MustAsync(BeUniqueEmail).WithMessage("Bu email adresi zaten kullanımda");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^(\+90|0)?[5][0-9]{9}$").WithMessage("Geçerli bir telefon numarası giriniz (5XXXXXXXXX)")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.EmployeeNumber)
                .Length(1, 100).WithMessage("Çalışan numarası en fazla 100 karakter olabilir")
                .MustAsync(BeUniqueEmployeeNumber).WithMessage("Bu çalışan numarası zaten kullanımda")
                .When(x => !string.IsNullOrEmpty(x.EmployeeNumber));

            RuleFor(x => x.PrimaryDepartmentId)
                .MustAsync(DepartmentExists).WithMessage("Belirtilen departman bulunamadı")
                .When(x => x.PrimaryDepartmentId.HasValue);

            RuleForEach(x => x.RoleIds)
                .MustAsync(RoleExists).WithMessage("Belirtilen rol bulunamadı");
        }

        private async Task<bool> BeUniqueRedmineUserId(int redmineUserId, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByRedmineUserIdAsync(redmineUserId);
            return user == null;
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.Users.IsEmailExistsAsync(email);
        }

        private async Task<bool> BeUniqueEmployeeNumber(string employeeNumber, CancellationToken cancellationToken)
        {
            return !await _unitOfWork.Users.IsEmployeeNumberExistsAsync(employeeNumber);
        }

        private async Task<bool> DepartmentExists(int? departmentId, CancellationToken cancellationToken)
        {
            if (!departmentId.HasValue) return true;
            return await _unitOfWork.Departments.ExistsAsync(departmentId.Value);
        }

        private async Task<bool> RoleExists(int roleId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Roles.ExistsAsync(roleId);
        }
    }
}