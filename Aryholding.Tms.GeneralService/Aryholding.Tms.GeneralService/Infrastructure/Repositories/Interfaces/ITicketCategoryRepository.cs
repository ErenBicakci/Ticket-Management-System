using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface ITicketCategoryRepository : IBaseRepository<TicketCategory>
    {
        Task<IEnumerable<TicketCategory>> GetAllByDepartmentAsync(int? departmentId);
        Task<TicketCategory?> GetByIdAsync(int id);
        Task<TicketCategory?> GetByCodeAsync(string code);
    }
}
