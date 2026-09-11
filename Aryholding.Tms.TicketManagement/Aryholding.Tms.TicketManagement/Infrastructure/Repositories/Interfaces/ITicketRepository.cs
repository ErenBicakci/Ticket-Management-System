
using Aryholding.Tms.TicketManagement.Application.DTOs;
using Aryholding.Tms.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Aryholding.Tms.TicketManagement.Infrastructure.Repositories.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<Ticket?> GetTicketByIdAsync(long id);

        Task<IEnumerable<Ticket>> GetTicketsWithFiltersAsync(
            string? severityCode,
            string? statusCode,
            string? categoryCode,
            string? username,
            string? orderDirection,
            int page,
            int pageSize);

        Task<IEnumerable<Ticket>> GetAssignedTicketsWithFiltersAsync(
            string? severityCode,
            string? statusCode,
            string? categoryCode,
            string? username,
            string? orderDirection,
            int page,
            int pageSize);

        Task<IEnumerable<Ticket>> GetDepartmentTicketsWithFiltersAsync(
            int departmentId,
            string? severityCode,
            string? statusCode,
            string? categoryCode,
            string? assignedUsername,
            string? orderDirection,
            int page,
            int pageSize);

        Task<UserTicketSummaryDto> GetUserTicketSummaryAsync(
            string username,
            CancellationToken cancellationToken = default);

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
