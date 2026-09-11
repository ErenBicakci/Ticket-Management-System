using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface ITicketStatusRepository : IBaseRepository<TicketStatus>
    {
        Task<TicketStatus?> GetByCodeAsync(string ticketStatusCode);
    }
}
