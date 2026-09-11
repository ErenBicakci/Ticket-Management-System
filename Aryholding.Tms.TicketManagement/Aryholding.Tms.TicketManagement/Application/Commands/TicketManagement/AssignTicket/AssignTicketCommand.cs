using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.AssignTicket
{
    public record AssignTicketCommand(string assignerUser,long ticketId, string assignedUser) : IRequest<TicketResponseDto>
    {
    }
}
