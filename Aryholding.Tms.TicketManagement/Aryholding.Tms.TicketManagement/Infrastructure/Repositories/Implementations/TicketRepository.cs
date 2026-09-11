using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(TicketDbContext context) : base(context)
        {
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsWithFiltersAsync(
            string? severityCode,
            string? statusCode,
            string? categoryCode,
            string? username,
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

            if (!string.IsNullOrWhiteSpace(severityCode))
                query = query.Where(t => t.Severity != null && t.Severity.TicketSeverityCode == severityCode);

            if (!string.IsNullOrWhiteSpace(statusCode))
                query = query.Where(t => t.TicketStatus != null && t.TicketStatus.TicketStatusCode == statusCode);

            if (!string.IsNullOrWhiteSpace(categoryCode))
                query = query.Where(t => t.Category != null && t.Category.TicketCategoryCode == categoryCode);

            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(t => t.User != null && t.User.UserName == username);

            int safePage = page < 1 ? 1 : page;
            int safePageSize = pageSize < 1 ? 10 : pageSize;

            bool asc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase);
            query = asc ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt);

            int skip = (safePage - 1) * safePageSize;
            query = query.Skip(skip).Take(safePageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAssignedTicketsWithFiltersAsync(
            string? severityCode,
            string? statusCode,
            string? categoryCode,
            string? username,
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

            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(t => t.AssignedUser != null && t.AssignedUser.UserName == username);

            if (!string.IsNullOrWhiteSpace(severityCode))
                query = query.Where(t => t.Severity != null && t.Severity.TicketSeverityCode == severityCode);

            if (!string.IsNullOrWhiteSpace(statusCode))
                query = query.Where(t => t.TicketStatus != null && t.TicketStatus.TicketStatusCode == statusCode);

            if (!string.IsNullOrWhiteSpace(categoryCode))
                query = query.Where(t => t.Category != null && t.Category.TicketCategoryCode == categoryCode);

            int safePage = page < 1 ? 1 : page;
            int safePageSize = pageSize < 1 ? 10 : pageSize;

            bool asc = string.Equals(orderDirection, "asc", StringComparison.OrdinalIgnoreCase);
            query = asc ? query.OrderBy(t => t.CreatedAt)
                        : query.OrderByDescending(t => t.CreatedAt);

            int skip = (safePage - 1) * safePageSize;
            query = query.Skip(skip).Take(safePageSize);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetDepartmentTicketsWithFiltersAsync(
            int departmentId,
            string? severityCode,
            string? statusCode,
            string? categoryCode,
            string? assignedUsername,
            string? orderDirection,
            int page,
            int pageSize)
        {
            int safePage = page < 1 ? 1 : page;
            int safePageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Tickets
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.AssignedUser)
                .Include(t => t.Category)
                    .ThenInclude(c => c.Department)
                .Include(t => t.PriorityLevel)
                .Include(t => t.TicketStatus)
                .Include(t => t.Severity)
                .Where(t => t.IsActive && t.Category != null && t.Category.DepartmentId == departmentId);

            if (!string.IsNullOrWhiteSpace(severityCode))
                query = query.Where(t => t.Severity != null && t.Severity.TicketSeverityCode == severityCode);

            if (!string.IsNullOrWhiteSpace(statusCode))
                query = query.Where(t => t.TicketStatus != null && t.TicketStatus.TicketStatusCode == statusCode);

            if (!string.IsNullOrWhiteSpace(categoryCode))
                query = query.Where(t => t.Category != null && t.Category.TicketCategoryCode == categoryCode);

            if (!string.IsNullOrWhiteSpace(assignedUsername))
                query = query.Where(t => t.AssignedUser != null && t.AssignedUser.UserName == assignedUsername);

            query = (orderDirection?.Trim().ToLowerInvariant()) switch
            {
                "asc" => query.OrderBy(t => t.CreatedAt),
                "desc" => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            return await query
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync();
        }

        public async Task<UserTicketSummaryDto> GetUserTicketSummaryAsync(string username, CancellationToken cancellationToken = default)
        {
            var statusCounts = await _context.Tickets
                .AsNoTracking()
                .Where(t => t.IsActive && t.User != null && t.User.UserName == username)
                .GroupBy(t => t.TicketStatus.TicketStatusCode)
                .Select(g => new { StatusCode = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.StatusCode, g => g.Count, cancellationToken);

            return new UserTicketSummaryDto
            {
                TotalTickets = statusCounts.Values.Sum(),
                PendingTickets = statusCounts.GetValueOrDefault("PENDING"),
                AcceptedTickets = statusCounts.GetValueOrDefault("ACCEPTED"),
                WaitingApprovalTickets = statusCounts.GetValueOrDefault("WAITING_APPROVAL"),
                ApprovedTickets = statusCounts.GetValueOrDefault("APPROVED")
            };
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
                .Include(t => t.Severity)
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

        public async Task<Ticket?> GetTicketByIdAsync(long id)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.AssignedUser)
                .Include(t => t.Category)
                    .ThenInclude(c => c.Department)
                .Include(t => t.PriorityLevel)
                .Include(t => t.TicketStatus)
                .Include(t => t.Severity)
                .Where(t => t.IsActive)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
