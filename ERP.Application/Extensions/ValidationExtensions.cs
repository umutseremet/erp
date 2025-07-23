namespace ERP.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPlateNumber(this string plateNumber)
        {
            if (string.IsNullOrWhiteSpace(plateNumber))
                return false;

            plateNumber = plateNumber.Trim().ToUpperInvariant();

            var patterns = new[]
            {
                @"^\d{2}\s[A-Z]{2,3}\s\d{2,4}$",
                @"^\d{2}[A-Z]{2,3}\d{2,4}$"
            };

            return patterns.Any(pattern => System.Text.RegularExpressions.Regex.IsMatch(plateNumber, pattern));
        }

        public static bool IsValidVinNumber(this string vinNumber)
        {
            if (string.IsNullOrWhiteSpace(vinNumber))
                return false;

            vinNumber = vinNumber.Trim().ToUpperInvariant();

            if (vinNumber.Length != 17)
                return false;

            var invalidChars = new[] { 'I', 'O', 'Q' };
            return !vinNumber.Any(c => invalidChars.Contains(c)) &&
                   vinNumber.All(c => char.IsLetterOrDigit(c));
        }

        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var cleaned = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Türkiye telefon numarası formatı
            return cleaned.Length == 10 && cleaned.StartsWith("5") ||
                   cleaned.Length == 12 && cleaned.StartsWith("905");
        }
    }
}
