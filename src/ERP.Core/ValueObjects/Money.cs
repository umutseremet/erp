namespace ERP.Core.ValueObjects
{
    public class Money : IEquatable<Money>
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency = "TRY")
        {
            if (amount < 0)
                throw new ArgumentException("Tutar negatif olamaz");

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Para birimi boş olamaz");

            Amount = Math.Round(amount, 2);
            Currency = currency.ToUpperInvariant();
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Farklı para birimleri toplanamaz");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Farklı para birimleri çıkarılamaz");

            return new Money(Amount - other.Amount, Currency);
        }

        public bool Equals(Money? other)
        {
            return other is not null && Amount == other.Amount && Currency == other.Currency;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Money);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public override string ToString()
        {
            return $"{Amount:F2} {Currency}";
        }

        public static Money operator +(Money left, Money right)
        {
            return left.Add(right);
        }

        public static Money operator -(Money left, Money right)
        {
            return left.Subtract(right);
        }
    }
}