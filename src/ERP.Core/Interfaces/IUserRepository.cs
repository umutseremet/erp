using ERP.Core.Entities;
using ERP.Core.Enums;

namespace ERP.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRedmineUserIdAsync(int redmineUserId);
        Task<User?> GetByEmployeeNumberAsync(string employeeNumber);
        Task<IEnumerable<User>> GetByStatusAsync(UserStatus status);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId);
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
        Task<bool> IsEmployeeNumberExistsAsync(string employeeNumber, int? excludeId = null);
    }
}