using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.RejectTicket
{
    public record RejectTicketCommand(string username, long ticketId) : IRequest<bool>
    {
    }
}
