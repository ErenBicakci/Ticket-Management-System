
using Aryholding.Tms.TicketManagement.Domain.Entities;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {

        Task<Department?> GetByCodeAsync(string? code);

        Task<int?> GetIdByCodeAsync(string? code);
    }
}
