using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class TicketSeverityRepository : BaseRepository<TicketSeverity>, ITicketSeverityRepository
    {
        public TicketSeverityRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<TicketSeverity?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.TicketSeverityCode == code);
        }
    }
}
