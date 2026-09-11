using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(TicketDbContext context) : base(context)
        {
        }

        public async Task<Department?> GetByCodeAsync(string code)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentCode == code);
        }

        public async Task<int?> GetIdByCodeAsync(string code)
        {
            return await _context.Departments.AsNoTracking()
                .Where(dp => dp.DepartmentCode == code)
                .Select(dp => (int?)dp.DepartmentId)
                .FirstOrDefaultAsync();
        }
    }
}
