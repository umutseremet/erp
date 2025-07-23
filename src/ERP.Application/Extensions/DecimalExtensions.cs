namespace ERP.Application.Extensions
{
    public static class DecimalExtensions
    {
        public static bool HasValue(this decimal? value)
        {
            return value.HasValue;
        }

        public static decimal? ToNullableDecimal(this decimal value)
        {
            return value == 0 ? null : value;
        }

        public static decimal GetValueOrDefault(this decimal? value, decimal defaultValue = 0)
        {
            return value ?? defaultValue;
        }
    }
}