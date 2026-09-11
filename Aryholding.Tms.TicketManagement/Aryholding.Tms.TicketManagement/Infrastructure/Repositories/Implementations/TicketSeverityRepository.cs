using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class TicketSeverityRepository : BaseRepository<TicketSeverity>, ITicketSeverityRepository
    {
        public TicketSeverityRepository(TicketDbContext context) : base(context)
        {
        }

        public async Task<TicketSeverity?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.TicketSeverityCode == code);
        }

        public async Task<int?> GetIdByCodeAsync(string? code)
        {
            return await _dbSet.AsNoTracking()
                .Where(tc => tc.TicketSeverityCode == code)
                .Select(tc => (int?)tc.Id)   
                .FirstOrDefaultAsync();      
        }
    }
}
