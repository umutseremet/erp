namespace ERP.Core.Exceptions
{
    public class VehicleNotFoundException : DomainException
    {
        public VehicleNotFoundException(int vehicleId) 
            : base($"ID'si {vehicleId} olan araç bulunamadı")
        {
        }

        public VehicleNotFoundException(string plateNumber) 
            : base($"Plaka numarası {plateNumber} olan araç bulunamadı")
        {
        }
    }
}