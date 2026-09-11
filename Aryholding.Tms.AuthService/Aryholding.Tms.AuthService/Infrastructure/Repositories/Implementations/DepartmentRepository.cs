using Microsoft.EntityFrameworkCore;
using Aryholding.Tms.AuthService.Domain.Entities;
using Aryholding.Tms.AuthService.Infrastructure.Data;
using Aryholding.Tms.AuthService.Infrastructure.Repositories.Interfaces;

namespace Aryholding.Tms.AuthService.Infrastructure.Repositories.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<Department?> GetByNameAsync(string departmentName)
        {
            return await _dbSet.FirstOrDefaultAsync(d => d.DepartmentName == departmentName);
        }

        public async Task<IEnumerable<Department>> GetActiveDepartmentsAsync()
        {
            return await _dbSet
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsIncludingInactiveAsync()
        {
            return await _dbSet
                .IgnoreQueryFilters()
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();
        }

        public async Task<bool> IsDepartmentNameUniqueAsync(string departmentName, int? excludeDepartmentId = null)
        {
            var query = _dbSet.Where(d => d.DepartmentName == departmentName);
            
            if (excludeDepartmentId.HasValue)
            {
                query = query.Where(d => d.DepartmentId != excludeDepartmentId.Value);
            }
            
            return !await query.AnyAsync();
        }

        public async Task<bool> CanDepartmentBeDeletedAsync(int departmentId)
        {
            var userCount = await _context.UserDepartmentRoles
                .Where(udr => udr.DepartmentId == departmentId)
                .CountAsync();
            
            return userCount == 0;
        }
    }
}