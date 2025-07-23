namespace ERP.Core.Exceptions
{
    public class InvalidFuelTransactionException : DomainException
    {
        public InvalidFuelTransactionException(string message) : base(message)
        {
        }

        public static InvalidFuelTransactionException InsufficientBalance(decimal availableBalance, decimal requestedAmount)
        {
            return new InvalidFuelTransactionException(
                $"Yetersiz bakiye. Mevcut: {availableBalance:C}, İstenen: {requestedAmount:C}");
        }

        public static InvalidFuelTransactionException InvalidQuantity(decimal quantity)
        {
            return new InvalidFuelTransactionException(
                $"Geçersiz yakıt miktarı: {quantity}. Miktar pozitif olmalıdır.");
        }

        public static InvalidFuelTransactionException FuelCardExpired(DateTime expiryDate)
        {
            return new InvalidFuelTransactionException(
                $"Yakıt kartı {expiryDate:dd.MM.yyyy} tarihinde süresi dolmuş.");
        }
    }
}