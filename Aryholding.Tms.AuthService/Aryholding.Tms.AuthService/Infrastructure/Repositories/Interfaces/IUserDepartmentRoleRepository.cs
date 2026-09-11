using Aryholding.Tms.AuthService.Domain.Entities;

namespace Aryholding.Tms.AuthService.Infrastructure.Repositories.Interfaces
{
    public interface IUserDepartmentRoleRepository : IBaseRepository<UserDepartmentRole>
    {
        Task<IEnumerable<UserDepartmentRole>> GetUserRolesWithDetailsAsync(string userId);
    }
}