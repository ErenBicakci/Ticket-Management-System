using Aryholding.Tms.TicketManagement.Domain.Entities;
using Aryholding.Tms.TicketManagement.Infrastructure.Data;
using Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Implementations
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(TicketDbContext context) : base(context)
        {


        }

        public async Task<IEnumerable<Comment>> GetCommentsByTicketIdWithPagination(long ticketId ,string? orderDirection, int page, int pageSize) {

            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Comments
                .AsNoTracking()
                .Where(t => t.TicketId == ticketId);
            

            query = (orderDirection?.Trim().ToLowerInvariant()) switch
            {
                "asc" => query.OrderBy(t => t.CreatedAt),
                "desc" => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderByDescending(t => t.CreatedAt)
            };

            return await query
                .Include(t => t.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
