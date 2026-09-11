using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface ITicketSeverityRepository : IBaseRepository<TicketSeverity>
    {
        Task<TicketSeverity?> GetByCodeAsync(string code);
    }
}
