using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class TicketCategoryRepository : BaseRepository<TicketCategory>, ITicketCategoryRepository
    {
        public TicketCategoryRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TicketCategory>> GetAllByDepartmentAsync(int? departmentId)
        {
            return await _dbSet
                .Include(tc => tc.Department)
                .Where(tc => tc.DepartmentId == departmentId || departmentId == null)
                .ToListAsync();
        }

        public async Task<TicketCategory?> GetByCodeAsync(string code)
        {
            return await _dbSet
                .Include(tc => tc.PriorityLevel)
                .Include(tc => tc.Department)
                .FirstOrDefaultAsync(tc => tc.TicketCategoryCode == code);
        }

        public async Task<TicketCategory?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(tc => tc.PriorityLevel)
                .Include(tc => tc.Department)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }
    }
}
