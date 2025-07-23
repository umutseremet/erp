using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<IEnumerable<Role>> GetSystemRolesAsync();
        Task<IEnumerable<Role>> GetRolesByUserAsync(int userId);
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
        Task<bool> HasUsersAsync(int roleId);
    }
}