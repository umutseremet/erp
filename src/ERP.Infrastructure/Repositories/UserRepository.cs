using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Enums;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.UserDepartments)
                    .ThenInclude(ud => ud.Department)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
        }

        public async Task<User?> GetByRedmineUserIdAsync(int redmineUserId)
        {
            return await _dbSet
                .Include(u => u.UserDepartments)
                    .ThenInclude(ud => ud.Department)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.RedmineUserId == redmineUserId);
        }

        public async Task<User?> GetByEmployeeNumberAsync(string employeeNumber)
        {
            return await _dbSet
                .Include(u => u.UserDepartments)
                    .ThenInclude(ud => ud.Department)
                .FirstOrDefaultAsync(u => u.EmployeeNumber == employeeNumber);
        }

        public async Task<IEnumerable<User>> GetByStatusAsync(UserStatus status)
        {
            return await _dbSet
                .Include(u => u.UserDepartments)
                    .ThenInclude(ud => ud.Department)
                .Where(u => u.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await GetByStatusAsync(UserStatus.Active);
        }

        public async Task<IEnumerable<User>> GetUsersByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .Include(u => u.UserDepartments)
                    .ThenInclude(ud => ud.Department)
                .Where(u => u.UserDepartments.Any(ud =>
                    ud.DepartmentId == departmentId && ud.IsActive))
                .ToListAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(u => u.Email == email.ToLowerInvariant());

            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsEmployeeNumberExistsAsync(string employeeNumber, int? excludeId = null)
        {
            var query = _dbSet.Where(u => u.EmployeeNumber == employeeNumber);

            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}