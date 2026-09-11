using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketHistoryManagement.GetTicketHistory
{
    public record GetTicketHistoryCommand(long ticketId , string username) : IRequest<IEnumerable<TicketHistoryResponseDto>>
    {
    }
}
