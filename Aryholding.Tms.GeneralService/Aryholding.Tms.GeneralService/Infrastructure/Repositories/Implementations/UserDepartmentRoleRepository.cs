using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class UserDepartmentRoleRepository : BaseRepository<UserDepartmentRole>, IUserDepartmentRoleRepository
    {
        public UserDepartmentRoleRepository(GeneralDbContext context) : base(context)
        {
        }
    }
}
