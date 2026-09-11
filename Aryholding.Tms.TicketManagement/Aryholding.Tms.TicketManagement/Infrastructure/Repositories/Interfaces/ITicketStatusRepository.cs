
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITicketStatusRepository : IBaseRepository<TicketStatus>
    {

        Task<TicketStatus?> GetByCodeAsync(string ticketStatusCode);
        Task<int?> GetIdByCodeAsync(string? code);
    }
}
