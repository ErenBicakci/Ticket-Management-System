using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class TicketCategoryRepository : BaseRepository<TicketCategory>, ITicketCategoryRepository
    {
        public TicketCategoryRepository(TicketDbContext context) : base(context)
        {

        }

        public async Task<TicketCategory?> GetByCodeAsync(string? code)
        {
            return await _dbSet
                .Include(tc => tc.PriorityLevel)
                .Include(tc => tc.Department)
                .FirstOrDefaultAsync(tc => tc.TicketCategoryCode == code);

        }

        public async Task<TicketCategory> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(tc => tc.PriorityLevel)
                .Include(tc => tc.Department)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<int?> GetIdByCodeAsync(string? code)
        {
            return await _dbSet.AsNoTracking()
                .Where(tc => tc.TicketCategoryCode == code)
                .Select(tc => (int?)tc.Id)        
                .FirstOrDefaultAsync();           
        }




    }
    
    
}
