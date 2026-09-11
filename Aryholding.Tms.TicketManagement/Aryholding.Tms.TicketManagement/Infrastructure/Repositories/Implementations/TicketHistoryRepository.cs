using Aryholding.Tms.TicketManagement.Application.Commands.StatsManagement.GetUserTicketStats;
using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class TicketHistoryRepository : BaseRepository<TicketHistory>, ITicketHistoryRepository
    {
        public TicketHistoryRepository(TicketDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TicketHistory>> GetAllByTicketIdAsync(long ticketId)
        {
            return await _dbSet
                .Where(th => th.TicketId == ticketId)
                .Include(th => th.TicketEventType)
                .Include(th => th.ChangedByUser)
                .ToListAsync();
        }

        public async Task<TicketHistory?> GetById(int id)
        {
            return await _dbSet
                .Include(th => th.TicketEventType)
                .FirstOrDefaultAsync(th => th.Id == (int?)id);
        }

        public async Task<List<UserTicketStatsDto>> GetUserTicketStatsAsync(
            DateTime? fromUtc,
            DateTime? toUtc,
            string? departmentCode,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Tickets
                .AsNoTracking()
                .Where(t => t.AssignedUserId != null);

            if (fromUtc.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= fromUtc.Value);
            }

            if (toUtc.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= toUtc.Value);
            }

            if (!string.IsNullOrEmpty(departmentCode))
            {
                query = query.Where(t => t.Category.Department.DepartmentCode == departmentCode);
            }

            var ticketData = await query
                .Select(t => new
                {
                    t.Id,
                    t.AssignedUserId,
                    t.AssignedUser.UserName,
                    AcceptedAt = t.TicketHistories
                        .Where(h => h.NewStatus.TicketStatusCode == "ACCEPTED")
                        .OrderBy(h => h.ChangedAt)
                        .Select(h => (DateTime?)h.ChangedAt)
                        .FirstOrDefault(),
                    WaitingAt = t.TicketHistories
                        .Where(h => h.NewStatus.TicketStatusCode == "WAITING_APPROVAL")
                        .OrderBy(h => h.ChangedAt)
                        .Select(h => (DateTime?)h.ChangedAt)
                        .FirstOrDefault(),
                    ApprovedAt = t.TicketHistories
                        .Where(h => h.NewStatus.TicketStatusCode == "APPROVED")
                        .OrderBy(h => h.ChangedAt)
                        .Select(h => (DateTime?)h.ChangedAt)
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            var stats = ticketData
                .GroupBy(t => new { t.AssignedUserId, t.UserName })
                .Select(g =>
                {
                    var acceptedTickets = g.Where(t => t.AcceptedAt != null).ToList();
                    var submittedTickets = g.Where(t => t.AcceptedAt != null && t.WaitingAt != null && t.WaitingAt >= t.AcceptedAt).ToList();
                    var completedTickets = g.Where(t => t.AcceptedAt != null && t.ApprovedAt != null && t.ApprovedAt >= t.AcceptedAt).ToList();

                    return new UserTicketStatsDto
                    {
                        UserId = g.Key.AssignedUserId!,
                        UserName = g.Key.UserName!,
                        AcceptedTicketCount = acceptedTickets.Count,
                        SubmittedForApprovalCount = g.Count(t => t.WaitingAt != null),
                        CompletedTicketCount = g.Count(t => t.ApprovedAt != null),
                        AvgMinutesToSubmitForApproval = submittedTickets.Any()
                            ? submittedTickets.Average(t => (t.WaitingAt!.Value - t.AcceptedAt!.Value).TotalMinutes)
                            : null,
                        AvgMinutesToCompletion = completedTickets.Any()
                            ? completedTickets.Average(t => (t.ApprovedAt!.Value - t.AcceptedAt!.Value).TotalMinutes)
                            : null
                    };
                })
                .OrderBy(s => s.UserName)
                .ToList();

            return stats;
        }
    }
}
