using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetDepartmentTickets
{
    public record GetDepartmentTicketsCommand(
        string? SeverityCode = null,
        string? StatusCode = null,
        string? CategoryCode = null,
        string? DepartmentCode = null,
        string? Username = null,
        string? AssignedUsername = null,
        int Page = 1,
        string OrderDirection = "desc"
    ) : IRequest<IEnumerable<TicketResponseDto>>;
}
