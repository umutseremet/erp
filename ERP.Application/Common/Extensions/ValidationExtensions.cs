using FluentValidation;
using ERP.Core.ValueObjects;
using System.Text.RegularExpressions;

namespace ERP.Application.Common.Extensions
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Türkiye plaka numarası formatını kontrol eder
        /// </summary>
        public static IRuleBuilder<T, string> IsValidPlateNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Plaka numarası boş olamaz")
                .Must(BeValidPlateNumber)
                .WithMessage("Geçersiz plaka numarası formatı (örn: 34 ABC 1234)");
        }

        /// <summary>
        /// VIN numarası formatını kontrol eder
        /// </summary>
        public static IRuleBuilder<T, string> IsValidVinNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("VIN numarası boş olamaz")
                .Length(17)
                .WithMessage("VIN numarası 17 karakter olmalıdır")
                .Must(BeValidVinNumber)
                .WithMessage("Geçersiz VIN numarası formatı");
        }

        /// <summary>
        /// Türkiye telefon numarası formatını kontrol eder
        /// </summary>
        public static IRuleBuilder<T, string> IsValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(BeValidPhoneNumber)
                .WithMessage("Geçersiz telefon numarası formatı (örn: 5XXXXXXXXX)");
        }

        /// <summary>
        /// Email formatını kontrol eder
        /// </summary>
        public static IRuleBuilder<T, string> IsValidEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("Email adresi boş olamaz")
                .EmailAddress()
                .WithMessage("Geçersiz email formatı");
        }

        /// <summary>
        /// Pozitif sayı kontrolü
        /// </summary>
        public static IRuleBuilder<T, decimal> IsPositive<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(0)
                .WithMessage("Değer pozitif olmalıdır");
        }

        /// <summary>
        /// Pozitif veya sıfır kontrolü
        /// </summary>
        public static IRuleBuilder<T, decimal> IsPositiveOrZero<T>(this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(0)
                .WithMessage("Değer negatif olamaz");
        }

        /// <summary>
        /// Geçmiş tarih kontrolü
        /// </summary>
        public static IRuleBuilder<T, DateTime> IsNotInFuture<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Tarih gelecekte olamaz");
        }

        /// <summary>
        /// Gelecek tarih kontrolü
        /// </summary>
        public static IRuleBuilder<T, DateTime> IsInFuture<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThan(DateTime.Now)
                .WithMessage("Tarih gelecekte olmalıdır");
        }

        /// <summary>
        /// Tarih aralığı kontrolü
        /// </summary>
        public static IRuleBuilder<T, DateTime> IsAfter<T>(this IRuleBuilder<T, DateTime> ruleBuilder, DateTime compareDate)
        {
            return ruleBuilder
                .GreaterThan(compareDate)
                .WithMessage($"Tarih {compareDate:dd.MM.yyyy} tarihinden sonra olmalıdır");
        }

        /// <summary>
        /// Yıl kontrolü (araç yılı için)
        /// </summary>
        public static IRuleBuilder<T, int> IsValidVehicleYear<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .GreaterThanOrEqualTo(1900)
                .WithMessage("Araç yılı 1900'den küçük olamaz")
                .LessThanOrEqualTo(DateTime.Now.Year + 1)
                .WithMessage("Araç yılı gelecek yıldan büyük olamaz");
        }

        /// <summary>
        /// Kilometre kontrolü (geriye gitmeme)
        /// </summary>
        public static IRuleBuilder<T, decimal> IsValidKilometer<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal currentKm)
        {
            return ruleBuilder
                .IsPositiveOrZero()
                .GreaterThanOrEqualTo(currentKm)
                .WithMessage($"Kilometre {currentKm:N0} km'den küçük olamaz");
        }

        /// <summary>
        /// Yakıt miktarı kontrolü
        /// </summary>
        public static IRuleBuilder<T, decimal> IsValidFuelQuantity<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal maxCapacity)
        {
            return ruleBuilder
                .IsPositive()
                .LessThanOrEqualTo(maxCapacity)
                .WithMessage($"Yakıt miktarı {maxCapacity:N2} litreyi aşamaz");
        }

        /// <summary>
        /// Maliyet kontrolü
        /// </summary>
        public static IRuleBuilder<T, decimal> IsValidCost<T>(this IRuleBuilder<T, decimal> ruleBuilder, decimal maxAmount = 1000000)
        {
            return ruleBuilder
                .IsPositiveOrZero()
                .LessThanOrEqualTo(maxAmount)
                .WithMessage($"Maliyet {maxAmount:C} tutarını aşamaz");
        }

        // Private helper methods
        private static bool BeValidPlateNumber(string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
                return false;

            plateNumber = plateNumber.Trim().ToUpperInvariant();

            var patterns = new[]
            {
                @"^\d{2}\s[A-Z]{2,3}\s\d{2,4}$",
                @"^\d{2}[A-Z]{2,3}\d{2,4}$"
            };

            return patterns.Any(pattern => Regex.IsMatch(plateNumber, pattern));
        }

        private static bool BeValidVinNumber(string vinNumber)
        {
            if (string.IsNullOrWhiteSpace(vinNumber) || vinNumber.Length != 17)
                return false;

            vinNumber = vinNumber.Trim().ToUpperInvariant();

            var invalidChars = new[] { 'I', 'O', 'Q' };
            return !vinNumber.Any(c => invalidChars.Contains(c)) &&
                   vinNumber.All(c => char.IsLetterOrDigit(c));
        }

        private static bool BeValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return true; // Nullable field

            var cleaned = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Türkiye telefon numarası formatı (5XXXXXXXXX veya 905XXXXXXXXX)
            return cleaned.Length == 10 && cleaned.StartsWith("5") ||
                   cleaned.Length == 12 && cleaned.StartsWith("905");
        }
    }
}