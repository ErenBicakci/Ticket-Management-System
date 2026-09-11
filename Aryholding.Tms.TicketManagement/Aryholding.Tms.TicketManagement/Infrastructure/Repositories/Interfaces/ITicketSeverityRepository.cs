
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITicketSeverityRepository : IBaseRepository<TicketSeverity>
    {
        Task<TicketSeverity?> GetByCodeAsync(string code);

        Task<int?> GetIdByCodeAsync(string? code);
    }
}
