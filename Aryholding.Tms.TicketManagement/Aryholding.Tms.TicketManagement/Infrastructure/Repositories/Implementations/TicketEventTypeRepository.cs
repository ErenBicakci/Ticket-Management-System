using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class TicketEventTypeRepository : BaseRepository<TicketEventType>, ITicketEventTypeRepository
    {
        public TicketEventTypeRepository(TicketDbContext context) : base(context)
        {
        }

        public async  Task<TicketEventType?> GetByCodeAsync(string? code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.EventTypeCode == code);
        }
    }
    
    
}
