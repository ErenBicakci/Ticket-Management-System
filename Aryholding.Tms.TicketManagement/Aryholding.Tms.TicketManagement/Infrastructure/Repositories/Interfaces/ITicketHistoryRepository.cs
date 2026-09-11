using Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetUserTicketStats;
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITicketHistoryRepository : IBaseRepository<TicketHistory>
    {
       Task<TicketHistory?> GetById(int id);

       Task<IEnumerable<TicketHistory>> GetAllByTicketIdAsync(long ticketId);

       Task<List<UserTicketStatsDto>> GetUserTicketStatsAsync(
           DateTime? fromUtc,
           DateTime? toUtc,
           string? departmentCode,
           CancellationToken cancellationToken = default);
    }
}
