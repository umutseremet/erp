using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<IEnumerable<Permission>> GetByModuleAsync(string module)
        {
            return await _dbSet
                .Where(p => p.Module == module)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByTypeAsync(PermissionType type)
        {
            return await _dbSet
                .Where(p => p.Type == type)
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetActivePermissionsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive)
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetSystemPermissionsAsync()
        {
            return await _dbSet
                .Where(p => p.IsSystemPermission)
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(int roleId)
        {
            return await _dbSet
                .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
                .OrderBy(p => p.Module)
                .ThenBy(p => p.Action)
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            var query = _dbSet.Where(p => p.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}