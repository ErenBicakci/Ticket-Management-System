using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.GetTickets
{
    public record GetTicketsCommand(
        string? SeverityCode = null,
        string? StatusCode = null,
        string? CategoryCode = null,
        string? Username = null,
        int Page = 1,                       
        string OrderDirection = "desc"      
    ) : IRequest<IEnumerable<TicketResponseDto>>;
}
