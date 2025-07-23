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
                new Permission("VehicleRead", "Ara� bilgilerini g�r�nt�leme", "Vehicle", "Read", PermissionType.Read),
                new Permission("VehicleWrite", "Ara� bilgilerini d�zenleme", "Vehicle", "Write", PermissionType.Write),
                new Permission("VehicleDelete", "Ara� silme", "Vehicle", "Delete", PermissionType.Delete),
                new Permission("VehicleManage", "Ara� y�netimi", "Vehicle", "Manage", PermissionType.Manage),
                
                // User permissions
                new Permission("UserRead", "Kullan�c� bilgilerini g�r�nt�leme", "User", "Read", PermissionType.Read),
                new Permission("UserWrite", "Kullan�c� bilgilerini d�zenleme", "User", "Write", PermissionType.Write),
                new Permission("UserDelete", "Kullan�c� silme", "User", "Delete", PermissionType.Delete),
                new Permission("UserManage", "Kullan�c� y�netimi", "User", "Manage", PermissionType.Manage),
                
                // Fuel permissions
                new Permission("FuelRead", "Yak�t bilgilerini g�r�nt�leme", "Fuel", "Read", PermissionType.Read),
                new Permission("FuelWrite", "Yak�t bilgilerini d�zenleme", "Fuel", "Write", PermissionType.Write),
                new Permission("FuelDelete", "Yak�t kayd� silme", "Fuel", "Delete", PermissionType.Delete),
                new Permission("FuelManage", "Yak�t y�netimi", "Fuel", "Manage", PermissionType.Manage),
                
                // Maintenance permissions
                new Permission("MaintenanceRead", "Bak�m bilgilerini g�r�nt�leme", "Maintenance", "Read", PermissionType.Read),
                new Permission("MaintenanceWrite", "Bak�m bilgilerini d�zenleme", "Maintenance", "Write", PermissionType.Write),
                new Permission("MaintenanceDelete", "Bak�m kayd� silme", "Maintenance", "Delete", PermissionType.Delete),
                new Permission("MaintenanceApprove", "Bak�m onaylama", "Maintenance", "Approve", PermissionType.Approve),
                new Permission("MaintenanceManage", "Bak�m y�netimi", "Maintenance", "Manage", PermissionType.Manage),
                
                // Department permissions
                new Permission("DepartmentRead", "Departman bilgilerini g�r�nt�leme", "Department", "Read", PermissionType.Read),
                new Permission("DepartmentWrite", "Departman bilgilerini d�zenleme", "Department", "Write", PermissionType.Write),
                new Permission("DepartmentDelete", "Departman silme", "Department", "Delete", PermissionType.Delete),
                new Permission("DepartmentManage", "Departman y�netimi", "Department", "Manage", PermissionType.Manage),
                
                // System permissions
                new Permission("SystemAdmin", "Sistem y�netimi", "System", "Admin", PermissionType.Admin),
                new Permission("SystemRead", "Sistem bilgilerini g�r�nt�leme", "System", "Read", PermissionType.Read),
                new Permission("SystemWrite", "Sistem ayarlar� d�zenleme", "System", "Write", PermissionType.Write),
                
                // Role permissions
                new Permission("RoleRead", "Rol bilgilerini g�r�nt�leme", "Role", "Read", PermissionType.Read),
                new Permission("RoleWrite", "Rol bilgilerini d�zenleme", "Role", "Write", PermissionType.Write),
                new Permission("RoleDelete", "Rol silme", "Role", "Delete", PermissionType.Delete),
                new Permission("RoleManage", "Rol y�netimi", "Role", "Manage", PermissionType.Manage),
                
                // Permission permissions
                new Permission("PermissionRead", "Yetki bilgilerini g�r�nt�leme", "Permission", "Read", PermissionType.Read),
                new Permission("PermissionWrite", "Yetki bilgilerini d�zenleme", "Permission", "Write", PermissionType.Write),
                new Permission("PermissionDelete", "Yetki silme", "Permission", "Delete", PermissionType.Delete),
                new Permission("PermissionManage", "Yetki y�netimi", "Permission", "Manage", PermissionType.Manage),
                
                // Insurance permissions
                new Permission("InsuranceRead", "Sigorta bilgilerini g�r�nt�leme", "Insurance", "Read", PermissionType.Read),
                new Permission("InsuranceWrite", "Sigorta bilgilerini d�zenleme", "Insurance", "Write", PermissionType.Write),
                new Permission("InsuranceDelete", "Sigorta kayd� silme", "Insurance", "Delete", PermissionType.Delete),
                new Permission("InsuranceManage", "Sigorta y�netimi", "Insurance", "Manage", PermissionType.Manage),
                
                // Report permissions
                new Permission("ReportRead", "Raporlar� g�r�nt�leme", "Report", "Read", PermissionType.Read),
                new Permission("ReportWrite", "Rapor olu�turma", "Report", "Write", PermissionType.Write),
                new Permission("ReportManage", "Rapor y�netimi", "Report", "Manage", PermissionType.Manage),
                
                // Location permissions
                new Permission("LocationRead", "Konum bilgilerini g�r�nt�leme", "Location", "Read", PermissionType.Read),
                new Permission("LocationWrite", "Konum bilgilerini d�zenleme", "Location", "Write", PermissionType.Write),
                new Permission("LocationManage", "Konum y�netimi", "Location", "Manage", PermissionType.Manage),
                
                // Notification permissions
                new Permission("NotificationRead", "Bildirimleri g�r�nt�leme", "Notification", "Read", PermissionType.Read),
                new Permission("NotificationWrite", "Bildirim olu�turma", "Notification", "Write", PermissionType.Write),
                new Permission("NotificationManage", "Bildirim y�netimi", "Notification", "Manage", PermissionType.Manage)
            };
        }
    }
}