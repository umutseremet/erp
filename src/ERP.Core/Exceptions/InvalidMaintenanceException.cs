namespace ERP.Core.Exceptions
{
    public class InvalidMaintenanceException : DomainException
    {
        public InvalidMaintenanceException(string message) : base(message)
        {
        }

        public static InvalidMaintenanceException AlreadyCompleted(int maintenanceId)
        {
            return new InvalidMaintenanceException(
                $"ID'si {maintenanceId} olan bakım zaten tamamlanmış.");
        }

        public static InvalidMaintenanceException InvalidScheduleDate(DateTime scheduledDate)
        {
            return new InvalidMaintenanceException(
                $"Geçersiz planlama tarihi: {scheduledDate:dd.MM.yyyy}. Tarih gelecekte olmalıdır.");
        }

        public static InvalidMaintenanceException VehicleNotAvailable(int vehicleId)
        {
            return new InvalidMaintenanceException(
                $"ID'si {vehicleId} olan araç bakım için uygun değil.");
        }
    }
}