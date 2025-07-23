namespace ERP.Core.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(int userId) 
            : base($"ID'si {userId} olan kullanıcı bulunamadı")
        {
        }

        public UserNotFoundException(string email) 
            : base($"Email adresi {email} olan kullanıcı bulunamadı")
        {
        }
    }
}