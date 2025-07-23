using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ERP.Core.ValueObjects;

namespace ERP.Infrastructure.Converters
{
    public class PlateNumberConverter : ValueConverter<PlateNumber, string>
    {
        public PlateNumberConverter() : base(
            plateNumber => plateNumber.Value,
            value => new PlateNumber(value))
        {
        }
    }
}