using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class TicketEventTypeRepository : BaseRepository<TicketEventType>, ITicketEventTypeRepository
    {
        public TicketEventTypeRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<TicketEventType?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.EventTypeCode == code);
        }
    }
}
