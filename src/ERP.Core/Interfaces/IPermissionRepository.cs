using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Core.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission?> GetByNameAsync(string name);
        Task<IEnumerable<Permission>> GetByModuleAsync(string module);
        Task<IEnumerable<Permission>> GetByTypeAsync(PermissionType type);
        Task<IEnumerable<Permission>> GetActivePermissionsAsync();
        Task<IEnumerable<Permission>> GetSystemPermissionsAsync();
        Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(int roleId);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
    }
}