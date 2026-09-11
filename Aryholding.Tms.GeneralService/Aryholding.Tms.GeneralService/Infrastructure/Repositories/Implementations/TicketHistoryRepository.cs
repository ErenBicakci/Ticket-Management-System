using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class TicketHistoryRepository : BaseRepository<TicketHistory>, ITicketHistoryRepository
    {
        public TicketHistoryRepository(GeneralDbContext context) : base(context)
        {
        }
    }
}
