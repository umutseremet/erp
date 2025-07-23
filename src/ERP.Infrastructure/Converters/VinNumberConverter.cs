using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ERP.Core.ValueObjects;

namespace ERP.Infrastructure.Converters
{
    public class VinNumberConverter : ValueConverter<VinNumber, string>
    {
        public VinNumberConverter() : base(
            vinNumber => vinNumber.Value,
            value => new VinNumber(value))
        {
        }
    }
}