using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Department>> GetAuthorizedDepartmentsAsync(string userId)
        {
            return await _context.UserDepartmentRoles
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Department!)
                .Where(d => d != null)
                .GroupBy(d => d.DepartmentId)
                .Select(g => g.First())
                .ToListAsync();
        }

        public override async Task<ApplicationUser?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(u => u.UserDepartmentRoles)
                    .ThenInclude(udr => udr.Department)
                .Include(u => u.UserDepartmentRoles)
                    .ThenInclude(udr => udr.PriorityLevel)
                .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(u => u.UserDepartmentRoles)
                    .ThenInclude(udr => udr.Department)
                .Include(u => u.UserDepartmentRoles)
                    .ThenInclude(udr => udr.PriorityLevel)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<UserDepartmentRole?> GetUserDepartmentRoleAsync(string userId, int departmentId)
        {
            return await _dbSet
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserDepartmentRoles)
                .Include(udr => udr.Department)
                .Include(udr => udr.PriorityLevel)
                .FirstOrDefaultAsync(udr => udr.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByDepartmentAsync(int departmentId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u => u.UserDepartmentRoles.Any(udr => udr.DepartmentId == departmentId))
                .Include(u => u.UserDepartmentRoles)
                    .ThenInclude(udr => udr.PriorityLevel)
                .Include(u => u.UserDepartmentRoles)
                    .ThenInclude(udr => udr.Department)
                .ToListAsync();
        }

        public async Task<bool> UserHasRoleInDepartmentAsync(string userId, int minLevel, int departmentId)
        {
            return await _context.UserDepartmentRoles
                .AsNoTracking()
                .AnyAsync(ud =>
                    ud.UserId == userId &&
                    ud.DepartmentId == departmentId &&
                    ud.PriorityLevel != null &&
                    ud.PriorityLevel.Level >= minLevel
                );
        }

    }
}
