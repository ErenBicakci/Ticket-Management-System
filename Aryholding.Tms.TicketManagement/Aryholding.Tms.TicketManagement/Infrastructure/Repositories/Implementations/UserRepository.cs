using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(TicketDbContext context) : base(context)
        {
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

            return await _dbSet.AsNoTracking()
                .Where(u => u.Id == userId)
                .SelectMany(u => u.UserDepartmentRoles)
                .Include(udr => udr.Department)
                .Include(udr => udr.PriorityLevel)
                .FirstOrDefaultAsync(udr => udr.DepartmentId == departmentId);
        }

        public async Task<string?> GetUserIdByUsernameAsync(string username)
        {
            return await _dbSet.AsNoTracking()
                .Where(u => u.UserName == username)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
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
