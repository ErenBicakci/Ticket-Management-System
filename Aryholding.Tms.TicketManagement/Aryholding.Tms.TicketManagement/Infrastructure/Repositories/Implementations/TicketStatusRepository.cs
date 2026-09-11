using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class TicketStatusRepository : BaseRepository<TicketStatus>, ITicketStatusRepository
    {
        public TicketStatusRepository(TicketDbContext context) : base(context)
        {
        }


        public async Task<TicketStatus?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts => ts.TicketStatusCode == code);
        }

        public async Task<int?> GetIdByCodeAsync(string? code)
        {
            var status = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(ts => ts.TicketStatusCode == code);
            return status?.Id;
        }
    }
}
