using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Infrastructure.Data.Seed
{
    public static class DefaultPermissions
    {
        public static List<Permission> GetDefaultPermissions()
        {
            return new List<Permission>
            {
                // Vehicle permissions
                new Permission("VehicleRead", "Araç bilgilerini görüntüleme", "Vehicle", "Read", PermissionType.Read),
                new Permission("VehicleWrite", "Araç bilgilerini düzenleme", "Vehicle", "Write", PermissionType.Write),
                new Permission("VehicleDelete", "Araç silme", "Vehicle", "Delete", PermissionType.Delete),
                new Permission("VehicleManage", "Araç yönetimi", "Vehicle", "Manage", PermissionType.Manage),
                
                // User permissions
                new Permission("UserRead", "Kullanýcý bilgilerini görüntüleme", "User", "Read", PermissionType.Read),
                new Permission("UserWrite", "Kullanýcý bilgilerini düzenleme", "User", "Write", PermissionType.Write),
                new Permission("UserDelete", "Kullanýcý silme", "User", "Delete", PermissionType.Delete),
                new Permission("UserManage", "Kullanýcý yönetimi", "User", "Manage", PermissionType.Manage),
                
                // Fuel permissions
                new Permission("FuelRead", "Yakýt bilgilerini görüntüleme", "Fuel", "Read", PermissionType.Read),
                new Permission("FuelWrite", "Yakýt bilgilerini düzenleme", "Fuel", "Write", PermissionType.Write),
                new Permission("FuelDelete", "Yakýt kaydý silme", "Fuel", "Delete", PermissionType.Delete),
                new Permission("FuelManage", "Yakýt yönetimi", "Fuel", "Manage", PermissionType.Manage),
                
                // Maintenance permissions
                new Permission("MaintenanceRead", "Bakým bilgilerini görüntüleme", "Maintenance", "Read", PermissionType.Read),
                new Permission("MaintenanceWrite", "Bakým bilgilerini düzenleme", "Maintenance", "Write", PermissionType.Write),
                new Permission("MaintenanceDelete", "Bakým kaydý silme", "Maintenance", "Delete", PermissionType.Delete),
                new Permission("MaintenanceApprove", "Bakým onaylama", "Maintenance", "Approve", PermissionType.Approve),
                new Permission("MaintenanceManage", "Bakým yönetimi", "Maintenance", "Manage", PermissionType.Manage),
                
                // Department permissions
                new Permission("DepartmentRead", "Departman bilgilerini görüntüleme", "Department", "Read", PermissionType.Read),
                new Permission("DepartmentWrite", "Departman bilgilerini düzenleme", "Department", "Write", PermissionType.Write),
                new Permission("DepartmentDelete", "Departman silme", "Department", "Delete", PermissionType.Delete),
                new Permission("DepartmentManage", "Departman yönetimi", "Department", "Manage", PermissionType.Manage),
                
                // System permissions
                new Permission("SystemAdmin", "Sistem yönetimi", "System", "Admin", PermissionType.Admin),
                new Permission("SystemRead", "Sistem bilgilerini görüntüleme", "System", "Read", PermissionType.Read),
                new Permission("SystemWrite", "Sistem ayarlarý düzenleme", "System", "Write", PermissionType.Write),
                
                // Role permissions
                new Permission("RoleRead", "Rol bilgilerini görüntüleme", "Role", "Read", PermissionType.Read),
                new Permission("RoleWrite", "Rol bilgilerini düzenleme", "Role", "Write", PermissionType.Write),
                new Permission("RoleDelete", "Rol silme", "Role", "Delete", PermissionType.Delete),
                new Permission("RoleManage", "Rol yönetimi", "Role", "Manage", PermissionType.Manage),
                
                // Permission permissions
                new Permission("PermissionRead", "Yetki bilgilerini görüntüleme", "Permission", "Read", PermissionType.Read),
                new Permission("PermissionWrite", "Yetki bilgilerini düzenleme", "Permission", "Write", PermissionType.Write),
                new Permission("PermissionDelete", "Yetki silme", "Permission", "Delete", PermissionType.Delete),
                new Permission("PermissionManage", "Yetki yönetimi", "Permission", "Manage", PermissionType.Manage),
                
                // Insurance permissions
                new Permission("InsuranceRead", "Sigorta bilgilerini görüntüleme", "Insurance", "Read", PermissionType.Read),
                new Permission("InsuranceWrite", "Sigorta bilgilerini düzenleme", "Insurance", "Write", PermissionType.Write),
                new Permission("InsuranceDelete", "Sigorta kaydý silme", "Insurance", "Delete", PermissionType.Delete),
                new Permission("InsuranceManage", "Sigorta yönetimi", "Insurance", "Manage", PermissionType.Manage),
                
                // Report permissions
                new Permission("ReportRead", "Raporlarý görüntüleme", "Report", "Read", PermissionType.Read),
                new Permission("ReportWrite", "Rapor oluþturma", "Report", "Write", PermissionType.Write),
                new Permission("ReportManage", "Rapor yönetimi", "Report", "Manage", PermissionType.Manage),
                
                // Location permissions
                new Permission("LocationRead", "Konum bilgilerini görüntüleme", "Location", "Read", PermissionType.Read),
                new Permission("LocationWrite", "Konum bilgilerini düzenleme", "Location", "Write", PermissionType.Write),
                new Permission("LocationManage", "Konum yönetimi", "Location", "Manage", PermissionType.Manage),
                
                // Notification permissions
                new Permission("NotificationRead", "Bildirimleri görüntüleme", "Notification", "Read", PermissionType.Read),
                new Permission("NotificationWrite", "Bildirim oluþturma", "Notification", "Write", PermissionType.Write),
                new Permission("NotificationManage", "Bildirim yönetimi", "Notification", "Manage", PermissionType.Manage)
            };
        }
    }
}