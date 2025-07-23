using ERP.Core.Entities;

namespace ERP.Infrastructure.Data.Seed
{

    public static class DefaultRoles
    {
        public static List<Role> GetDefaultRoles()
        {
            return new List<Role>
            {
                new Role("SystemAdmin", "Sistem yöneticisi - Tüm yetkilere sahip"),
                new Role("FleetManager", "Filo yöneticisi - Araç yönetimi yetkisi"),
                new Role("FleetUser", "Filo kullanýcýsý - Temel araç iþlemleri"),
                new Role("MaintenanceManager", "Bakým yöneticisi - Bakým yönetimi yetkisi"),
                new Role("MaintenanceUser", "Bakým kullanýcýsý - Bakým kayýtlarý"),
                new Role("Driver", "Þoför - Araç kullaným yetkisi"),
                new Role("Viewer", "Görüntüleyici - Sadece okuma yetkisi"),
                new Role("DepartmentManager", "Departman yöneticisi - Departman yönetimi"),
                new Role("HR", "Ýnsan kaynaklarý - Kullanýcý yönetimi"),
                new Role("Accountant", "Muhasebeci - Mali raporlar")
            };
        }
    }
}