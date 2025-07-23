using System.ComponentModel.DataAnnotations;
using ERP.Core.ValueObjects;

namespace ERP.Core.Entities
{
    public class VehicleLocationHistory : BaseEntity
    {
        public int VehicleId { get; private set; }
        
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        
        [StringLength(500)]
        public string? Address { get; private set; }
        
        public DateTime RecordedAt { get; private set; }
        
        public decimal? Speed { get; private set; } // km/h
        
        [StringLength(10)]
        public string? Direction { get; private set; } // N, NE, E, SE, S, SW, W, NW
        
        public decimal? Altitude { get; private set; }
        
        public decimal? Accuracy { get; private set; }
        
        [StringLength(100)]
        public string? DataSource { get; private set; } // GPS, Mobile, Manual

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;

        protected VehicleLocationHistory() { }

        public VehicleLocationHistory(int vehicleId, decimal latitude, decimal longitude, 
                                    DateTime recordedAt, string? address = null)
        {
            VehicleId = vehicleId;
            SetCoordinates(latitude, longitude);
            RecordedAt = recordedAt;
            Address = address?.Trim();
        }

        public void SetCoordinates(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new ArgumentException("Latitude -90 ile 90 arasında olmalıdır");

            if (longitude < -180 || longitude > 180)
                throw new ArgumentException("Longitude -180 ile 180 arasında olmalıdır");

            Latitude = latitude;
            Longitude = longitude;
            UpdateTimestamp();
        }

        public void SetSpeed(decimal? speed)
        {
            if (speed.HasValue && speed < 0)
                throw new ArgumentException("Hız negatif olamaz");

            Speed = speed;
            UpdateTimestamp();
        }

        public void SetDirection(string? direction)
        {
            var validDirections = new[] { "N", "NE", "E", "SE", "S", "SW", "W", "NW" };
            
            if (!string.IsNullOrWhiteSpace(direction) && !validDirections.Contains(direction.ToUpperInvariant()))
                throw new ArgumentException("Geçersiz yön değeri");

            Direction = direction?.ToUpperInvariant();
            UpdateTimestamp();
        }

        public void SetAltitude(decimal? altitude)
        {
            Altitude = altitude;
            UpdateTimestamp();
        }

        public void SetAccuracy(decimal? accuracy)
        {
            if (accuracy.HasValue && accuracy < 0)
                throw new ArgumentException("Doğruluk değeri negatif olamaz");

            Accuracy = accuracy;
            UpdateTimestamp();
        }

        public void SetDataSource(string? dataSource)
        {
            DataSource = dataSource?.Trim();
            UpdateTimestamp();
        }

        public Address GetAddress()
        {
            return new Address(Latitude, Longitude, Address ?? "", "", "");
        }

        public static double CalculateDistance(VehicleLocationHistory from, VehicleLocationHistory to)
        {
            const double R = 6371; // Earth's radius in kilometers
            
            var dLat = ToRadians((double)(to.Latitude - from.Latitude));
            var dLon = ToRadians((double)(to.Longitude - from.Longitude));
            
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians((double)from.Latitude)) * Math.Cos(ToRadians((double)to.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            
            return R * c;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}