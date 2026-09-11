using Aryholding.Tms.GeneralService.Domain.Entities;
using Aryholding.Tms.GeneralService.Infrastructure.Data;
using Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Implementations
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(GeneralDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ticket>> GetAssignedTicketsBySeverityAndStatusAndCategoryAndUserWithPagination(int? severityId, int? statusId, int? categoryId, string? userId, string? orderDirection, int page, int pageSize)
        {
            var query = _context.Tickets
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.AssignedUser)
                .Include(t => t.Category)
                .Include(t => t.PriorityLevel)
                .Include(t => t.TicketStatus)
                .Where(t => t.IsActive && t.AssignedUserId == userId);

            if (severityId.HasValue)
                query = query.Where(t => t.SeverityId == severityId.Value);

            if (statusId.HasValue)
                query = query.Where(t => t.TicketStatusId == statusId.Value);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            int safePage = page < 1 ? 1 : page;
            int safePageSize = pageSize < 1 ? 10 : pageSize;

            bool asc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase);
            query = asc ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt);

            int skip = (safePage - 1) * safePageSize;
            query = query.Skip(skip).Take(safePageSize);

            return await query.ToListAsync();
        }

        public override async Task<Ticket?> GetByIdAsync(object id)
        {
            return await _dbSet
                .Include(t => t.User)
                .Include(t => t.AssignedUser)
                .Include(t => t.Category)
                .Include(t => t.PriorityLevel)
                .Include(t => t.TicketStatus)
                .Include(t => t.Severity)
                .FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySeverityAndStatusAndCategoryAndUserWithPagination(
            int? severityId,
            int? statusId,
            int? categoryId,
            string? userId,
            string? orderDirection,
            int page,
            int pageSize)
        {
            var query = _context.Tickets
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.AssignedUser)
                .Include(t => t.Category)
                .Include(t => t.PriorityLevel)
                .Include(t => t.TicketStatus)
                .Include(t => t.Severity)
                .Where(t => t.IsActive);

            if (severityId.HasValue)
                query = query.Where(t => t.SeverityId == severityId.Value);

            if (statusId.HasValue)
                query = query.Where(t => t.TicketStatusId == statusId.Value);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(userId))
                query = query.Where(t => t.UserId == userId);

            int safePage = page < 1 ? 1 : page;
            int safePageSize = pageSize < 1 ? 10 : pageSize;

            bool asc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase);
            query = asc ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt);

            int skip = (safePage - 1) * safePageSize;
            query = query.Skip(skip).Take(safePageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByDepartmentAndSeverityAndStatusAndCategoryAndAssignedUserWithPaginationAsync(int? departmentId, int? severityId, int? statusId, int? categoryId, string? assignedUserId, string? orderDirection, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Tickets
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.AssignedUser)
                .Include(t => t.Category)
                    .ThenInclude(c => c.Department)
                .Include(t => t.PriorityLevel)
                .Include(t => t.TicketStatus)
                .Include(t => t.Severity)
                .Where(t => t.IsActive);

            if (departmentId.HasValue)
                query = query.Where(t => t.Category != null && t.Category.DepartmentId == departmentId.Value);

            if (severityId.HasValue)
                query = query.Where(t => t.SeverityId == severityId.Value);

            if (statusId.HasValue)
                query = query.Where(t => t.TicketStatusId == statusId.Value);

            if (categoryId.HasValue)
                query = query.Where(t => t.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(assignedUserId))
                query = query.Where(t => t.AssignedUserId == assignedUserId);

            query = (orderDirection?.Trim().ToLowerInvariant()) switch
            {
                "asc" => query.OrderBy(t => t.CreatedAt),
                "desc" => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
