using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        Task<bool> UserHasRoleInDepartmentAsync(string userId, int minLevel, int departmentId);

        Task<ApplicationUser?> GetByUsernameAsync(string username);

        Task<UserDepartmentRole?> GetUserDepartmentRoleAsync(string userId, int departmentId);
        Task<string?> GetUserIdByUsernameAsync(string username);
    }
}
