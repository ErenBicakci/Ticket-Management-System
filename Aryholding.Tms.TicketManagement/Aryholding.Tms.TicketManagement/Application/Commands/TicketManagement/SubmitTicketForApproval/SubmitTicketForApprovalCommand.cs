using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.SubmitTicketForApproval
{
    public record SubmitTicketForApprovalCommand(string username,long ticketId) : IRequest<bool>
    {
    }
}
