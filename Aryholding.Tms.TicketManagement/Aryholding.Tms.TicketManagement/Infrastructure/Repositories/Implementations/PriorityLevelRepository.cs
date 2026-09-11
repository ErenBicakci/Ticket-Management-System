using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class PriorityLevelRepository : BaseRepository<PriorityLevel>, IPriorityLevelRepository
    {
        public PriorityLevelRepository(TicketDbContext context) : base(context)
        {
        }

        public async Task<PriorityLevel?> GetByLevelAsync(int? level)
        {
            return await _dbSet.FirstOrDefaultAsync(pl => pl.Level == level);
        }
    }
}
