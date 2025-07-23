using ERP.Core.Exceptions;

namespace ERP.Core.ValueObjects
{
    public class PhoneNumber : IEquatable<PhoneNumber>
    {
        public string Value { get; private set; }

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Telefon numarası boş olamaz");

            value = CleanPhoneNumber(value);

            if (!IsValidFormat(value))
                throw new DomainException("Geçersiz telefon numarası formatı");

            Value = value;
        }

        private static string CleanPhoneNumber(string phoneNumber)
        {
            return new string(phoneNumber.Where(char.IsDigit).ToArray());
        }

        private static bool IsValidFormat(string phoneNumber)
        {
            // Türkiye telefon numarası formatı (5XXXXXXXXX veya 905XXXXXXXXX)
            return phoneNumber.Length == 10 && phoneNumber.StartsWith("5") ||
                   phoneNumber.Length == 12 && phoneNumber.StartsWith("905");
        }

        public string GetFormattedNumber()
        {
            if (Value.Length == 10)
                return $"+90 {Value[0]} {Value.Substring(1, 3)} {Value.Substring(4, 2)} {Value.Substring(6, 2)}";
            
            return $"+{Value.Substring(0, 2)} {Value[2]} {Value.Substring(3, 3)} {Value.Substring(6, 2)} {Value.Substring(8, 2)}";
        }

        public bool Equals(PhoneNumber? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PhoneNumber);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return GetFormattedNumber();
        }
    }
}