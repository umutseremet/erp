using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Seed
{

    public static class DefaultRoles
    {
        public static List<Role> GetDefaultRoles()
        {
            return new List<Role>
            {
                new Role("SystemAdmin", "Sistem y�neticisi - T�m yetkilere sahip"),
                new Role("FleetManager", "Filo y�neticisi - Ara� y�netimi yetkisi"),
                new Role("FleetUser", "Filo kullan�c�s� - Temel ara� i�lemleri"),
                new Role("MaintenanceManager", "Bak�m y�neticisi - Bak�m y�netimi yetkisi"),
                new Role("MaintenanceUser", "Bak�m kullan�c�s� - Bak�m kay�tlar�"),
                new Role("Driver", "�of�r - Ara� kullan�m yetkisi"),
                new Role("Viewer", "G�r�nt�leyici - Sadece okuma yetkisi"),
                new Role("DepartmentManager", "Departman y�neticisi - Departman y�netimi"),
                new Role("HR", "�nsan kaynaklar� - Kullan�c� y�netimi"),
                new Role("Accountant", "Muhasebeci - Mali raporlar")
            };
        }
    }
}