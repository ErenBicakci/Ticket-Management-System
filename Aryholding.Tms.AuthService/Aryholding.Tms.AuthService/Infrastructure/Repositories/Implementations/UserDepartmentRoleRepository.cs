using Aryholding.Tms.AuthService.Domain.Entities;
using Aryholding.Tms.AuthService.Infrastructure.Data;
using Aryholding.Tms.AuthService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.AuthService.Infrastructure.Repositories.Implementations
{
    public class UserDepartmentRoleRepository : BaseRepository<UserDepartmentRole>, IUserDepartmentRoleRepository
    {
        public UserDepartmentRoleRepository(AuthDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserDepartmentRole>> GetUserRolesWithDetailsAsync(string userId)
        {
            return await _dbSet
                .Include(udr => udr.Department)
                .Include(udr => udr.PriorityLevel)
                .Where(udr => udr.UserId == userId)
                .OrderBy(udr => udr.CreatedAt)
                .ToListAsync();
        }
    }
}