using System.ComponentModel.DataAnnotations;
using ERP.Core.Enums;
using ERP.Core.ValueObjects;

namespace ERP.Core.Entities
{
    public class FuelTransaction : BaseEntity
    {
        public int VehicleId { get; private set; }
        public int? FuelCardId { get; private set; }
        
        public DateTime TransactionDate { get; private set; }
        
        public decimal Quantity { get; private set; } // Litre
        public decimal UnitPrice { get; private set; } // Litre başına fiyat
        public decimal TotalAmount { get; private set; }
        
        public FuelType FuelType { get; private set; }
        
        [StringLength(200)]
        public string? StationName { get; private set; }
        
        [StringLength(500)]
        public string? StationAddress { get; private set; }
        
        public decimal VehicleKm { get; private set; }
        
        [StringLength(1000)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual Vehicle Vehicle { get; set; } = null!;
        public virtual FuelCard? FuelCard { get; set; }

        protected FuelTransaction() { }

        public FuelTransaction(int vehicleId, DateTime transactionDate, decimal quantity, 
                             decimal unitPrice, FuelType fuelType, decimal vehicleKm, 
                             int? fuelCardId = null, string? stationName = null)
        {
            VehicleId = vehicleId;
            TransactionDate = transactionDate;
            SetQuantityAndPrice(quantity, unitPrice);
            FuelType = fuelType;
            VehicleKm = vehicleKm;
            FuelCardId = fuelCardId;
            StationName = stationName?.Trim();
        }

        public void SetQuantityAndPrice(decimal quantity, decimal unitPrice)
        {
            if (quantity <= 0)
                throw new ArgumentException("Yakıt miktarı pozitif olmalıdır");
            
            if (unitPrice <= 0)
                throw new ArgumentException("Birim fiyat pozitif olmalıdır");

            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalAmount = Math.Round(quantity * unitPrice, 2);
            UpdateTimestamp();
        }

        public void SetStationInfo(string? stationName, string? stationAddress)
        {
            StationName = stationName?.Trim();
            StationAddress = stationAddress?.Trim();
            UpdateTimestamp();
        }

        public void SetNotes(string? notes)
        {
            Notes = notes?.Trim();
            UpdateTimestamp();
        }

        public Money GetTotalAmountAsMoney(string currency = "TRY")
        {
            return new Money(TotalAmount, currency);
        }
    }
}