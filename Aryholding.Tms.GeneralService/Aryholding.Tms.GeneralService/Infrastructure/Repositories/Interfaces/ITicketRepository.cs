using Aryholding.Tms.GeneralService.Domain.Entities;

namespace Aryholding.Tms.GeneralService.Infrastructure.Repositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetTicketsBySeverityAndStatusAndCategoryAndUserWithPagination(
            int? severityId,
            int? statusId,
            int? categoryId,
            string? userId,
            string? orderDirection,
            int page,
            int pageSize);

        Task<IEnumerable<Ticket>> GetAssignedTicketsBySeverityAndStatusAndCategoryAndUserWithPagination(
            int? severityId,
            int? statusId,
            int? categoryId,
            string? userId,
            string? orderDirection,
            int page,
            int pageSize);

        Task<IEnumerable<Ticket>> GetTicketsByDepartmentAndSeverityAndStatusAndCategoryAndAssignedUserWithPaginationAsync(
            int? departmentId,
            int? severityId,
            int? statusId,
            int? categoryId,
            string? assignedUserId,
            string? orderDirection,
            int page,
            int pageSize);
    }
}
