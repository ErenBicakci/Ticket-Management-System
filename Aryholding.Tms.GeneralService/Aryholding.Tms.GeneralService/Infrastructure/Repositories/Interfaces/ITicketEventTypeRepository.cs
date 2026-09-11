using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface ITicketEventTypeRepository : IBaseRepository<TicketEventType>
    {
        Task<TicketEventType?> GetByCodeAsync(string code);
    }
}
