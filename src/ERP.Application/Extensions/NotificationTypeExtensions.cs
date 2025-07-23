namespace ERP.Application.Extensions
{
    public static class NotificationTypeExtensions
    {
        public static ERP.Core.Enums.NotificationType ToNotificationType(this string notificationType)
        {
            return notificationType.ToLowerInvariant() switch
            {
                "maintenancedue" => ERP.Core.Enums.NotificationType.MaintenanceDue,
                "inspectiondue" => ERP.Core.Enums.NotificationType.InspectionDue,
                "insuranceexpiring" => ERP.Core.Enums.NotificationType.InsuranceExpiring,
                "licenseexpiring" => ERP.Core.Enums.NotificationType.LicenseExpiring,
                "fuelcardexpiring" => ERP.Core.Enums.NotificationType.FuelCardExpiring,
                "vehicleassigned" => ERP.Core.Enums.NotificationType.VehicleAssigned,
                "vehiclereturned" => ERP.Core.Enums.NotificationType.VehicleReturned,
                "accidentreported" => ERP.Core.Enums.NotificationType.AccidentReported,
                "servicecompleted" => ERP.Core.Enums.NotificationType.ServiceCompleted,
                "overduereturn" => ERP.Core.Enums.NotificationType.OverdueReturn,
                _ => ERP.Core.Enums.NotificationType.MaintenanceDue
            };
        }
    }
}