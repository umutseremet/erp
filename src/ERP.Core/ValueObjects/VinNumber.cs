using ERP.Core.Exceptions;

namespace ERP.Core.ValueObjects
{
    public class VinNumber : IEquatable<VinNumber>
    {
        public string Value { get; private set; }

        public VinNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("VIN numarası boş olamaz");

            value = value.Trim().ToUpperInvariant();

            if (!IsValidFormat(value))
                throw new DomainException("Geçersiz VIN formatı");

            Value = value;
        }

        private static bool IsValidFormat(string vinNumber)
        {
            if (vinNumber.Length != 17)
                return false;

            var invalidChars = new[] { 'I', 'O', 'Q' };
            return !vinNumber.Any(c => invalidChars.Contains(c)) &&
                   vinNumber.All(c => char.IsLetterOrDigit(c));
        }

        public bool Equals(VinNumber? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as VinNumber);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(VinNumber vinNumber)
        {
            return vinNumber.Value;
        }

        public static explicit operator VinNumber(string value)
        {
            return new VinNumber(value);
        }
    }
}