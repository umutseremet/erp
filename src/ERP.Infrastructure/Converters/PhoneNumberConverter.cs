using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ERP.Core.ValueObjects;

namespace ERP.Infrastructure.Converters
{
    public class PhoneNumberConverter : ValueConverter<PhoneNumber?, string?>
    {
        public PhoneNumberConverter() : base(
            phoneNumber => phoneNumber != null ? phoneNumber.Value : null,
            value => value != null ? new PhoneNumber(value) : null)
        {
        }
    }
}