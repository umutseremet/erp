using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;
using ERP.Core.ValueObjects;

namespace ERP.Core.Entities
{
    public class Vehicle : BaseEntity
    {
        public string PlateNumber { get; private set; } = string.Empty;
        public string VinNumber { get; private set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Brand { get; private set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; private set; } = string.Empty;

        public int Year { get; private set; }

        [StringLength(50)]
        public string? Color { get; private set; }

        public VehicleType Type { get; private set; }
        public FuelType FuelType { get; private set; }
        public VehicleStatus Status { get; private set; } = VehicleStatus.Available;

        public decimal CurrentKm { get; private set; }
        public decimal FuelCapacity { get; private set; }
        public decimal EngineSize { get; private set; }

        public DateTime PurchaseDate { get; private set; }
        public decimal PurchasePrice { get; private set; }

        public int? AssignedUserId { get; private set; }

        [StringLength(1000)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual User? AssignedUser { get; set; }
        public virtual ICollection<VehicleMaintenance> Maintenances { get; set; } = new List<VehicleMaintenance>();
        public virtual ICollection<VehicleInspection> Inspections { get; set; } = new List<VehicleInspection>();
        public virtual ICollection<VehicleLicense> Licenses { get; set; } = new List<VehicleLicense>();
        public virtual ICollection<VehicleLocationHistory> LocationHistory { get; set; } = new List<VehicleLocationHistory>();
        public virtual ICollection<FuelTransaction> FuelTransactions { get; set; } = new List<FuelTransaction>();
        public virtual ICollection<TireChange> TireChanges { get; set; } = new List<TireChange>();

        protected Vehicle() { }

        public Vehicle(string plateNumber, string vinNumber, string brand, string model, int year,
                      VehicleType type, FuelType fuelType, DateTime purchaseDate, decimal purchasePrice)
        {
            SetPlateNumber(plateNumber);
            SetVinNumber(vinNumber);
            SetBrandModel(brand, model);
            SetYear(year);
            Type = type;
            FuelType = fuelType;
            PurchaseDate = purchaseDate;
            PurchasePrice = purchasePrice;
        }

        public void SetPlateNumber(string plateNumber)
        {
            var plate = new PlateNumber(plateNumber);
            PlateNumber = plate.Value;
            UpdateTimestamp();
        }

        public void SetVinNumber(string vinNumber)
        {
            var vin = new VinNumber(vinNumber);
            VinNumber = vin.Value;
            UpdateTimestamp();
        }

        public void SetBrandModel(string brand, string model)
        {
            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Marka boş olamaz");

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model boş olamaz");

            Brand = brand.Trim();
            Model = model.Trim();
            UpdateTimestamp();
        }

        public void SetYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year + 1)
                throw new ArgumentException("Geçersiz yıl");

            Year = year;
            UpdateTimestamp();
        }

        public void UpdateKilometer(decimal km)
        {
            if (km < CurrentKm)
                throw new ArgumentException("Kilometre geriye doğru güncellenemez");

            CurrentKm = km;
            UpdateTimestamp();
        }

        public void AssignToUser(int userId)
        {
            if (Status != VehicleStatus.Available)
                throw new InvalidOperationException("Araç atama için uygun değil");

            AssignedUserId = userId;
            Status = VehicleStatus.Assigned;
            UpdateTimestamp();
        }

        public void ReturnFromUser()
        {
            AssignedUserId = null;
            Status = VehicleStatus.Available;
            UpdateTimestamp();
        }

        public void SetStatus(VehicleStatus status)
        {
            Status = status;
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public void SetColor(string color)
        {
            Color = color;
            UpdateTimestamp();
        }

        public void SetFuelCapacity(decimal fuelCapacity)
        {
            FuelCapacity = fuelCapacity;
            UpdateTimestamp();
        }

        public void SetEngineSize(decimal engineSize)
        {
            EngineSize = engineSize;
            UpdateTimestamp();
        }
    }
}