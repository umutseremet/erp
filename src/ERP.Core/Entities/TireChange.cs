using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Entities
{
    public class TireChange : BaseEntity
    {
        public int VehicleId { get; private set; }
        
        public DateTime ChangeDate { get; private set; }
        
        public decimal VehicleKm { get; private set; }
        
        [StringLength(100)]
        public string TireBrand { get; private set; } = string.Empty;
        
        [StringLength(50)]
        public string TireSize { get; private set; } = string.Empty;
        
        public int Quantity { get; private set; }
        
        [StringLength(200)]
        public string? ServiceProvider { get; private set; }
        
        public decimal? Cost { get; private set; }
        
        [StringLength(1000)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;

        protected TireChange() { }

        public TireChange(int vehicleId, DateTime changeDate, decimal vehicleKm, 
                        string tireBrand, string tireSize, int quantity)
        {
            VehicleId = vehicleId;
            ChangeDate = changeDate;
            VehicleKm = vehicleKm;
            SetTireBrand(tireBrand);
            SetTireSize(tireSize);
            SetQuantity(quantity);
        }

        public void SetTireBrand(string tireBrand)
        {
            if (string.IsNullOrWhiteSpace(tireBrand))
                throw new ArgumentException("Lastik markası boş olamaz");

            TireBrand = tireBrand.Trim();
            UpdateTimestamp();
        }

        public void SetTireSize(string tireSize)
        {
            if (string.IsNullOrWhiteSpace(tireSize))
                throw new ArgumentException("Lastik boyutu boş olamaz");

            TireSize = tireSize.Trim();
            UpdateTimestamp();
        }

        public void SetQuantity(int quantity)
        {
            if (quantity <= 0 || quantity > 6)
                throw new ArgumentException("Lastik adedi 1-6 arasında olmalıdır");

            Quantity = quantity;
            UpdateTimestamp();
        }

        public void SetCost(decimal? cost)
        {
            if (cost.HasValue && cost < 0)
                throw new ArgumentException("Maliyet negatif olamaz");

            Cost = cost;
            UpdateTimestamp();
        }

        public void SetServiceProvider(string? serviceProvider)
        {
            ServiceProvider = serviceProvider?.Trim();
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }
    }
}