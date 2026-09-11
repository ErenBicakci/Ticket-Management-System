using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface IPriorityLevelRepository : IBaseRepository<PriorityLevel>
    {
        Task<PriorityLevel?> GetByLevelAsync(int level);
    }
}
