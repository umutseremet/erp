using ERP.Core.Entities;

namespace ERP.Core.Interfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> GetByCodeAsync(string code);
        Task<IEnumerable<Department>> GetActiveAsync();
        Task<IEnumerable<Department>> GetSubDepartmentsAsync(int parentId);
        Task<IEnumerable<Department>> GetRootDepartmentsAsync();
        Task<bool> IsCodeExistsAsync(string code, int? excludeId = null);
        Task<bool> HasSubDepartmentsAsync(int departmentId);
        Task<bool> HasUsersAsync(int departmentId);
    }
}