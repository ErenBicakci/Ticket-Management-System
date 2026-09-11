
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITicketCategoryRepository : IBaseRepository<TicketCategory>
    {

        Task<TicketCategory?> GetByIdAsync(int id);
        Task<TicketCategory?> GetByCodeAsync(string? code);

        Task<int?> GetIdByCodeAsync(string? code);
    }
}
