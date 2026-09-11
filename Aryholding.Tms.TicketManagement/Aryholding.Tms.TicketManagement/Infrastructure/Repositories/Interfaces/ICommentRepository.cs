
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {

        Task<IEnumerable<Comment>> GetCommentsByTicketIdWithPagination(
                long ticketId,
                string? orderDirection,
                int page,
                int pageSize);
    }
}
