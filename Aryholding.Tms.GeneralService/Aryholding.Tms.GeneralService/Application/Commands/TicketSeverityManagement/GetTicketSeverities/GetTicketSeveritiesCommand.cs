using Aryholding.Tms.GeneralService.Application.DTOs;
using MediatR;

namespace Aryholding.Tms.GeneralService.Application.Commands.TicketSeverityManagement.GetTicketSeverities
{
    public record GetTicketSeveritiesCommand : IRequest<IEnumerable<TicketSeverityResponseDTO>>
    {
    }
}
