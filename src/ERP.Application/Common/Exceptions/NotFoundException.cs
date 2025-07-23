using ERP.Application.Common.Exceptions;
 
namespace ERP.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Kayıt bulunamadı")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string name, object key)
            : base($"{name} ({key}) bulunamadı")
        {
        }

        public NotFoundException(string name, object key, Exception innerException)
            : base($"{name} ({key}) bulunamadı", innerException)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}