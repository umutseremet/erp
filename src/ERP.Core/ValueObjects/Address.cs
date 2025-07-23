namespace ERP.Core.ValueObjects
{
    public class Address : IEquatable<Address>
    {
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }
        public string? PostalCode { get; private set; }

        public Address(decimal latitude, decimal longitude, string street, string city, string district, string? postalCode = null)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Latitude -90 ile 90 arasında olmalıdır");

            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude -180 ile 180 arasında olmalıdır");

            Latitude = latitude;
            Longitude = longitude;
            Street = street?.Trim() ?? string.Empty;
            City = city?.Trim() ?? throw new ArgumentException("Şehir boş olamaz");
            District = district?.Trim() ?? throw new ArgumentException("İlçe boş olamaz");
            PostalCode = postalCode?.Trim();
        }

        public string GetFullAddress()
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(Street))
                parts.Add(Street);
            
            if (!string.IsNullOrEmpty(District))
                parts.Add(District);
            
            if (!string.IsNullOrEmpty(City))
                parts.Add(City);
            
            if (!string.IsNullOrEmpty(PostalCode))
                parts.Add(PostalCode);

            return string.Join(", ", parts);
        }

        public bool Equals(Address? other)
        {
            return other is not null &&
                   Latitude == other.Latitude &&
                   Longitude == other.Longitude &&
                   Street == other.Street &&
                   City == other.City &&
                   District == other.District &&
                   PostalCode == other.PostalCode;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Address);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude, Street, City, District, PostalCode);
        }

        public override string ToString()
        {
            return GetFullAddress();
        }
    }
}