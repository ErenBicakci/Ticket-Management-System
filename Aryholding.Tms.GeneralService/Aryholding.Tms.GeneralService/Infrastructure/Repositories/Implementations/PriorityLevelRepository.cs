using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class PriorityLevelRepository : BaseRepository<PriorityLevel>, IPriorityLevelRepository
    {
        public PriorityLevelRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<PriorityLevel?> GetByLevelAsync(int level)
        {
            return await _dbSet.FirstOrDefaultAsync(pl => pl.Level == level);
        }
    }
}
