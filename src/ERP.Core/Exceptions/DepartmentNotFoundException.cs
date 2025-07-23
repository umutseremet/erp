namespace ERP.Core.Exceptions
{
    public class DepartmentNotFoundException : DomainException
    {
        public DepartmentNotFoundException(int departmentId) 
            : base($"ID'si {departmentId} olan departman bulunamadı")
        {
        }

        public DepartmentNotFoundException(string departmentCode) 
            : base($"Kodu {departmentCode} olan departman bulunamadı")
        {
        }
    }
}