
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITicketEventTypeRepository: IBaseRepository<TicketEventType>
    {


        Task<TicketEventType?> GetByCodeAsync(string? code);
    }
}
