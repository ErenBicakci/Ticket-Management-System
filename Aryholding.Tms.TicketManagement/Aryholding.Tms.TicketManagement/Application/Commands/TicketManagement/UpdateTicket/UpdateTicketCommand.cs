
using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.UpdateTicket
{
    public record UpdateTicketCommand(UpdateTicketDto Dto, string Username) : IRequest<TicketResponseDto>;
}
