using System.ComponentModel.DataAnnotations;

namespace ERP.Core.Entities
{
    public class FuelCard : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string CardNumber { get; private set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string ProviderName { get; private set; } = string.Empty;
        
        public DateTime ExpiryDate { get; private set; }
        
        public decimal CreditLimit { get; private set; }
        
        public decimal CurrentBalance { get; private set; }
        
        public bool IsActive { get; private set; } = true;
        
        public int? AssignedVehicleId { get; private set; }
        
        [StringLength(500)]
        public string? Notes { get; private set; }

        // Navigation Properties
        public virtual Vehicle? AssignedVehicle { get; set; }
        public virtual ICollection<FuelTransaction> Transactions { get; set; } = new List<FuelTransaction>();

        protected FuelCard() { }

        public FuelCard(string cardNumber, string providerName, DateTime expiryDate, decimal creditLimit)
        {
            SetCardNumber(cardNumber);
            SetProviderName(providerName);
            ExpiryDate = expiryDate;
            SetCreditLimit(creditLimit);
            CurrentBalance = creditLimit;
        }

        public void SetCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                throw new ArgumentException("Kart numarası boş olamaz");

            CardNumber = cardNumber.Trim();
            UpdateTimestamp();
        }

        public void SetProviderName(string providerName)
        {
            if (string.IsNullOrWhiteSpace(providerName))
                throw new ArgumentException("Sağlayıcı adı boş olamaz");

            ProviderName = providerName.Trim();
            UpdateTimestamp();
        }

        public void SetCreditLimit(decimal creditLimit)
        {
            if (creditLimit <= 0)
                throw new ArgumentException("Kredi limiti pozitif olmalıdır");

            CreditLimit = creditLimit;
            UpdateTimestamp();
        }

        public void UpdateBalance(decimal amount)
        {
            CurrentBalance += amount;
            UpdateTimestamp();
        }

        public void AssignToVehicle(int vehicleId)
        {
            AssignedVehicleId = vehicleId;
            UpdateTimestamp();
        }

        public void UnassignFromVehicle()
        {
            AssignedVehicleId = null;
            UpdateTimestamp();
        }

        public void Activate()
        {
            IsActive = true;
            UpdateTimestamp();
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdateTimestamp();
        }

        public bool IsExpired => DateTime.UtcNow > ExpiryDate;
        public bool HasSufficientBalance(decimal amount) => CurrentBalance >= amount;
    }
}