using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class TicketStatusRepository : BaseRepository<TicketStatus>, ITicketStatusRepository
    {
        public TicketStatusRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<TicketStatus?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.TicketStatusCode == code);
        }
    }
}
