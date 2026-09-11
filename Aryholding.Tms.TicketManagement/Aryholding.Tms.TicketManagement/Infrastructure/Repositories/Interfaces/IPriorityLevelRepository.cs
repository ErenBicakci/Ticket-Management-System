
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface IPriorityLevelRepository : IBaseRepository<PriorityLevel>
    {

        Task<PriorityLevel?> GetByLevelAsync(int? level);
    }
}
