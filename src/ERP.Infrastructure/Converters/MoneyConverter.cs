using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ERP.Core.ValueObjects;
using System.Text.Json;

namespace ERP.Infrastructure.Converters
{
    public class MoneyConverter : ValueConverter<Money, string>
    {
        public MoneyConverter() : base(
            money => SerializeMoney(money),
            json => DeserializeMoney(json))
        {
        }

        private static string SerializeMoney(Money money)
        {
            return JsonSerializer.Serialize(new { Amount = money.Amount, Currency = money.Currency });
        }

        private static Money DeserializeMoney(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return new Money(0, "TRY");

                var data = JsonSerializer.Deserialize<MoneyData>(json);

                if (data == null)
                    return new Money(0, "TRY");

                return new Money(data.Amount, data.Currency ?? "TRY");
            }
            catch
            {
                return new Money(0, "TRY");
            }
        }

        private class MoneyData
        {
            public decimal Amount { get; set; }
            public string? Currency { get; set; } = "TRY";
        }
    }
}