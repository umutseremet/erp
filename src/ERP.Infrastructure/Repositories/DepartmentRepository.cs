using Microsoft.EntityFrameworkCore;
using ERP.Core.Entities;
using ERP.Core.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Department?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .Include(d => d.ParentDepartment)
                .Include(d => d.SubDepartments)
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<IEnumerable<Department>> GetActiveAsync()
        {
            return await _dbSet
                .Include(d => d.ParentDepartment)
                .Where(d => d.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetSubDepartmentsAsync(int parentId)
        {
            return await _dbSet
                .Include(d => d.SubDepartments)
                .Where(d => d.ParentDepartmentId == parentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetRootDepartmentsAsync()
        {
            return await _dbSet
                .Include(d => d.SubDepartments)
                .Where(d => d.ParentDepartmentId == null)
                .ToListAsync();
        }

        public async Task<bool> IsCodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _dbSet.Where(d => d.Code == code);

            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasSubDepartmentsAsync(int departmentId)
        {
            return await _dbSet.AnyAsync(d => d.ParentDepartmentId == departmentId);
        }

        public async Task<bool> HasUsersAsync(int departmentId)
        {
            return await _context.UserDepartments
                .AnyAsync(ud => ud.DepartmentId == departmentId && ud.IsActive);
        }
    }
}