using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.ApproveTicket
{
    public record ApproveTicketCommand(string username,long ticketId) : IRequest<bool>
    {
    }
}
