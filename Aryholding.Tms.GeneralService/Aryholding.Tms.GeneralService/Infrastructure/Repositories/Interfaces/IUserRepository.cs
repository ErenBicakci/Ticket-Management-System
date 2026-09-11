using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<bool> UserHasRoleInDepartmentAsync(string userId, int minLevel, int departmentId);

        Task<ApplicationUser?> GetByUsernameAsync(string username);

        Task<UserDepartmentRole?> GetUserDepartmentRoleAsync(string userId, int departmentId);

        Task<IEnumerable<Department>> GetAuthorizedDepartmentsAsync(string userId);

        Task<IEnumerable<ApplicationUser>> GetUsersByDepartmentAsync(int departmentId);
    }
}
