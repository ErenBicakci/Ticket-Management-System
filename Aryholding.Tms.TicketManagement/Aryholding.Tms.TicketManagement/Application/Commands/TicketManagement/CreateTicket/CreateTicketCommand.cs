using Aryholding.Tms.TicketManagement.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.TicketManagement.Application.Commands.TicketManagement.CreateTicket
{
    public record CreateTicketCommand(CreateTicketDto Dto, string Username) : IRequest<TicketResponseDto>;
}
