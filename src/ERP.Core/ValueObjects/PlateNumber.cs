using ERP.Core.Exceptions;

namespace ERP.Core.ValueObjects
{
    public class PlateNumber : IEquatable<PlateNumber>
    {
        public string Value { get; private set; }

        public PlateNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Plaka numarası boş olamaz");

            value = value.Trim().ToUpperInvariant();

            if (!IsValidFormat(value))
                throw new DomainException("Geçersiz plaka formatı");

            Value = value;
        }

        private static bool IsValidFormat(string plateNumber)
        {
            var patterns = new[]
            {
                @"^\d{2}\s[A-Z]{2,3}\s\d{2,4}$",
                @"^\d{2}[A-Z]{2,3}\d{2,4}$"
            };

            return patterns.Any(pattern => System.Text.RegularExpressions.Regex.IsMatch(plateNumber, pattern));
        }

        public bool Equals(PlateNumber? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PlateNumber);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(PlateNumber plateNumber)
        {
            return plateNumber.Value;
        }

        public static explicit operator PlateNumber(string value)
        {
            return new PlateNumber(value);
        }
    }
}