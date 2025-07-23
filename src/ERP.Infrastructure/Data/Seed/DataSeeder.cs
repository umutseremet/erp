using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Data.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedPermissionsAsync();
            await SeedRolesAsync();
            await SeedDepartmentsAsync();
            await _context.SaveChangesAsync();
        }

        private async Task SeedPermissionsAsync()
        {
            if (_context.Permissions.Any())
                return;

            var permissions = DefaultPermissions.GetDefaultPermissions();

            foreach (var permission in permissions)
            {
                permission.MarkAsSystem();
                await _context.Permissions.AddAsync(permission);
            }
        }

        private async Task SeedRolesAsync()
        {
            if (_context.Roles.Any())
                return;

            var roles = DefaultRoles.GetDefaultRoles();

            foreach (var role in roles)
            {
                role.MarkAsSystem();
                await _context.Roles.AddAsync(role);
            }
        }

        private async Task SeedDepartmentsAsync()
        {
            if (_context.Departments.Any())
                return;

            var departments = new[]
            {
                new Department("Bilgi ��lem", "Bilgi teknolojileri departman�", "B�LG�"),
                new Department("�nsan Kaynaklar�", "�nsan kaynaklar� departman�", "�K"),
                new Department("Muhasebe", "Muhasebe departman�", "MUH"),
                new Department("Ara� Takip", "Ara� takip departman�", "ARA�")
            };

            foreach (var department in departments)
            {
                await _context.Departments.AddAsync(department);
            }
        }
    }
}